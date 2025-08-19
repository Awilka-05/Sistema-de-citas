
using Microsoft.Extensions.Logging;
using SistemaGestionCitas.Application.Validators;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Enums;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Domain.Interfaces.Services;
using SistemaGestionCitas.Domain.Result_Pattern;
using SistemaGestionCitas.Infrastructure.Services.Correo.Factory;
using SistemaGestionCitas.Infrastructure.Services.Correo.Strategy;

namespace SistemaGestionCitas.Application.UseCases
{
    public class ReservarCita : IReservarCitaService
    {
        private readonly ICitaRepository _citaRepository;
        private readonly ICitaValidator _citaValidator;
        private readonly ILogger<ReservarCita> _logger;

        public ReservarCita(
            ICitaRepository citaRepository,
            ICitaValidator citaValidator,
            ILogger<ReservarCita> logger)
        {
            _citaRepository = citaRepository;
            _citaValidator = citaValidator;
            _logger = logger;
        }

        public async Task<Result<Cita>> ReservarCitaAsync(Usuario usuario, Cita cita)
        {
            var validacion = await _citaValidator.ValidarCreacionAsync(cita);
            if (validacion.IsFailure)
            {
                _logger.LogWarning("No se pudo reservar la cita: {Error}", validacion.Error);
                return Result<Cita>.Failure(validacion.Error);
            }

            cita.Estado = EstadoCita.Confirmada;

            await _citaRepository.AddAsync(cita);
            _logger.LogInformation($"Cita '{cita.IdCita}' reservada exitosamente.");

            var citaCompleta = await _citaRepository.GetByIdAsync(cita.IdCita);

            if (citaCompleta == null)
            {
                _logger.LogError("Fallo al cargar la cita completa para el correo.");
                return Result<Cita>.Failure("No se pudo confirmar la cita: error interno.");
            }

            var resultadoEstrategia = CorreoEstrategiaFactory.FactoryCorreo("confirmacion");

            if (resultadoEstrategia.IsSuccess)
            {
                var estrategiaConfirmacion = resultadoEstrategia.Value;
                var context = new CorreoContext();
                context.SetStrategy(estrategiaConfirmacion);

                await context.EjecutarAsync(citaCompleta, usuario);
            }
            else
            {
                _logger.LogError("No se pudo crear la estrategia de correo: {Error}", resultadoEstrategia.Error);
            }

            return Result<Cita>.Success(citaCompleta);
        }
    }
}
