using Microsoft.Extensions.Logging;
using SistemaGestionCitas.Application.DTOs.Requests;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Enums;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Domain.Result_Pattern;

namespace SistemaGestionCitas.Application.Validators
{
    public class CitaValidator : ICitaValidator
    {
        private readonly ICitaRepository _citaRepository;
        private readonly IConfiguracionTurnoRepository _configuracionTurnosRepository;
        private readonly ILogger<CitaValidator> _logger;

        public CitaValidator(
            ICitaRepository citaRepository,
            IConfiguracionTurnoRepository configuracionTurnosRepository,
            ILogger<CitaValidator> logger)
        {
            _citaRepository = citaRepository;
            _configuracionTurnosRepository = configuracionTurnosRepository;
            _logger = logger;
        }

        public async Task<Result<Cita>> ValidarCreacionAsync(Cita entity)
        {
            var turno = await _configuracionTurnosRepository.GetByIdAsync(entity.TurnoId);

            if (turno == null)
            {
                _logger.LogError("Fallo al crear una cita. El turno con ID {TurnoId} no existe.", entity.TurnoId);
                return Result<Cita>.Failure("El turno especificado no existe.");
            }

            //var fechaCompletaTurno = turno.FechaInicio.Add(turno.Horario.HoraInicio);

            //if (fechaCompletaTurno < DateTime.Now)
            //{
            //    _logger.LogError("Fallo al crear una cita. El turno ya paso");
            //    return Result<Cita>.Failure("No se pueden agendar citas en un horario que ya paso.");
            //}

            //if (fechaCompletaTurno > DateTime.Now.AddDays(7))
            //{
            //    _logger.LogError("Fallo al crear una cita. El turno está demasiado lejos.");
            //    return Result<Cita>.Failure("Las citas solo se pueden agendar con un máximo de 7 días de antelación.");
            //}

            var citasDelUsuario = await _citaRepository.GetCitasByUsuarioAsync(entity.IdUsuario);

            var otraCitaMismoDia = citasDelUsuario.Any(cita =>
            {
                var fechaDeOtraCita = cita.ConfiguracionTurno.FechaInicio.Date;
                return fechaDeOtraCita == turno.FechaInicio.Date;
            });

            if (otraCitaMismoDia)
            {
                _logger.LogError(
                    "Fallo al crear una cita. El usuario {IdUsuario} ya tiene otra cita agendada para este día.",
                    entity.IdUsuario);
                return Result<Cita>.Failure("Ya tienes una cita agendada para este día.");
            }
            // Contar el número de citas existentes para el turno
            var citasExistentes = (await _citaRepository.GetByFechaAsync(turno.FechaInicio)).Count();

            // Validar si el cupo está disponible
            if (citasExistentes >= turno.CantidadEstaciones)
            {
                _logger.LogError(
                    "Fallo al crear una cita. El turno con ID {TurnoId} ha alcanzado el límite de citas.",
                    entity.TurnoId);
                return Result<Cita>.Failure("No hay estaciones disponibles para este turno.");
            }

            // Si todas las validaciones pasan, la operación es un éxito.
            return Result<Cita>.Success(new Cita());
        }

        public async Task<Result<Cita>> ValidarCancelacionAsync(int citaId, Usuario usuario)
        {
            var cita = await _citaRepository.GetByIdAsync(citaId);
            if (cita == null)
                return Result<Cita>.Failure("La cita no existe.");

            var fechaCita = cita.ConfiguracionTurno.FechaInicio.Add(cita.ConfiguracionTurno.Horario.HoraInicio);
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
