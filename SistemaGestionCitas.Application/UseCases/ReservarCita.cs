using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaGestionCitas.Application.Validators;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Enums;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Domain.Interfaces.Services;
using SistemaGestionCitas.Domain.Result_Pattern;
using SistemaGestionCitas.Infrastructure.Persistence.BdContext;
using SistemaGestionCitas.Infrastructure.Services.Correo.Factory;
using SistemaGestionCitas.Infrastructure.Services.Correo.Strategy;

public class ReservarCita : IReservarCitaService
{
    private readonly ICitaRepository _citaRepository;
    private readonly ICitaValidator _citaValidator;
    private readonly ILogger<ReservarCita> _logger;
    private readonly SistemaCitasDbContext _context; // Agrega el DbContext aquí

    public ReservarCita(
        ICitaRepository citaRepository,
        ICitaValidator citaValidator,
        ILogger<ReservarCita> logger,
        SistemaCitasDbContext context) // Inyecta el DbContext
    {
        _citaRepository = citaRepository;
        _citaValidator = citaValidator;
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Cita>> ReservarCitaAsync(Cita cita)
    {
        // 1. Cargar las relaciones necesarias para el proceso.
        var configuracionTurno = await _context.ConfiguracionesTurnos
            .Include(ct => ct.Franjas)
            .FirstOrDefaultAsync(ct => ct.TurnoId == cita.TurnoId);

        if (configuracionTurno == null)
        {
            return Result<Cita>.Failure("El turno seleccionado no existe.");
        }

        var franjaSeleccionada = configuracionTurno.Franjas
            .FirstOrDefault(f => f.FranjaId == cita.FranjaId);

        if (franjaSeleccionada == null)
        {
            return Result<Cita>.Failure("La franja seleccionada no existe.");
        }

        cita.ConfiguracionTurno = configuracionTurno;

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

        // Enviar correo de confirmación
        var resultadoEstrategia = CorreoEstrategiaFactory.FactoryCorreo("confirmacion");
        if (resultadoEstrategia.IsSuccess)
        {
            var estrategiaConfirmacion = resultadoEstrategia.Value;
            var context = new CorreoContext();
            context.SetStrategy(estrategiaConfirmacion);

            await context.EjecutarAsync(citaCompleta, citaCompleta.Usuario, franjaSeleccionada);
        }
        else
        {
            _logger.LogError("No se pudo crear la estrategia de correo: {Error}", resultadoEstrategia.Error);
        }

        return Result<Cita>.Success(citaCompleta);
    }
}