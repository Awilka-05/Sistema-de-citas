using Microsoft.Extensions.Logging;
using SistemaGestionCitas.Application.DTOs.Requests;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Enums;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Domain.Result_Pattern;
using SistemaGestionCitas.Infrastructure.Repositories;

namespace SistemaGestionCitas.Application.Validators
{
    public class CitaValidator : ICitaValidator
    {
        private readonly ICitaRepository _citaRepository;
        private readonly IConfiguracionTurnoRepository _configuracionTurnoRepository;
        private readonly ILogger<CitaValidator> _logger;

        public CitaValidator(
            ICitaRepository citaRepository,
            IConfiguracionTurnoRepository configuracionTurnosRepository,
            ILogger<CitaValidator> logger)
        {
            _citaRepository = citaRepository;
            _configuracionTurnoRepository = configuracionTurnosRepository;
            _logger = logger;
        }

        public async Task<Result<Cita>> ValidarCreacionAsync(Cita entity)
        {
            var turno = await _configuracionTurnoRepository.GetByIdAsync(entity.TurnoId);
            if (turno == null)
            {
                _logger.LogError("Fallo en la validación de la cita. El turno con ID {TurnoId} no fue encontrado.", entity.TurnoId);
                return Result<Cita>.Failure("El turno seleccionado no es válido.");
            }

            DateTime fechaYHoraInicioNuevaCita = turno.FechaInicio.Add(turno.Horario.HoraInicio.ToTimeSpan());
            DateTime fechaYHoraFinNuevaCita = fechaYHoraInicioNuevaCita.AddMinutes(turno.DuracionMinutos);

            var citasConfirmadasDelUsuario = await _citaRepository.GetCitasByUsuarioAsync(entity.IdUsuario);

            // Verificar si alguna cita existente del usuario se solapa con la nueva.
            bool seSolapan = citasConfirmadasDelUsuario.Any(citaExistente =>
            {
                DateTime fechaYHoraInicioExistente = citaExistente.ConfiguracionTurno.FechaInicio.Add(citaExistente.ConfiguracionTurno.Horario.HoraInicio.ToTimeSpan());
                DateTime fechaYHoraFinExistente = fechaYHoraInicioExistente.AddMinutes(citaExistente.ConfiguracionTurno.DuracionMinutos);

                return (fechaYHoraInicioNuevaCita < fechaYHoraFinExistente) && (fechaYHoraFinNuevaCita > fechaYHoraInicioExistente);
            });

            if (seSolapan)
            {
                _logger.LogWarning("Fallo al crear una cita. El usuario {IdUsuario} ya tiene otra cita agendada con este horario.", entity.IdUsuario);
                return Result<Cita>.Failure("Ya tienes una cita agendada en un horario que se solapa con este.");
            }

            var citasExistentes = await _citaRepository.CountByTurnoIdAsync(entity.TurnoId);

            if (citasExistentes >= turno.CantidadEstaciones)
            {
                _logger.LogWarning("Fallo en la validación de la cita. El turno con ID {TurnoId} ha alcanzado el límite de citas ({CantidadEstaciones}).", entity.TurnoId, turno.CantidadEstaciones);
                return Result<Cita>.Failure("No hay estaciones disponibles para este turno.");
            }

            return Result<Cita>.Success(new Cita());
        }

        public async Task<Result<Cita>> ValidarCancelacionAsync(int citaId, Usuario usuario)
        {
            var cita = await _citaRepository.GetByIdAsync(citaId);
            if (cita == null)
                return Result<Cita>.Failure("La cita no existe.");

            var fechaCita = cita.ConfiguracionTurno.FechaInicio.Add(cita.ConfiguracionTurno.Horario.HoraInicio.ToTimeSpan());
            if (fechaCita <= DateTime.Now)
            {
                _logger.LogError("Fallo al cancelar una cita. La cita con ID {CitaId} ya ha pasado.", citaId);
                return Result<Cita>.Failure("No se puede cancelar una cita que ya ha pasado.");
            }

            if (cita.Estado != EstadoCita.Confirmada)
            {
                _logger.LogWarning($"Intento de cancelar cita {citaId} cuyo estado no es confirmado.");
                return Result<Cita>.Failure("Solo se pueden cancelar citas confirmadas.");
            }

            return Result<Cita>.Success(cita); 
        }
    }
}
