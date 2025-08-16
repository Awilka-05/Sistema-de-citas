using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SistemaGestionCitas.Application.DTOs.Requests;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Domain.Result_Pattern;

namespace SistemaGestionCitas.Application.Validators
{
    public class CitaValidator
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

        public async Task<Result<object>> ValidarCreacionAsync(CrearCitaDto crearCita)
        {
            var turno = await _configuracionTurnosRepository.GetByIdAsync(crearCita.TurnoId);

            if (turno == null)
            {
                _logger.LogError("Fallo al crear una cita. El turno con ID {TurnoId} no existe.", crearCita.TurnoId);
                return Result<object>.Failure("El turno especificado no existe.");
            }

            var fechaCompletaTurno = turno.FechaInicio.Add(turno.Horario.HoraInicio);

            if (fechaCompletaTurno < DateTime.Now)
            {
                _logger.LogError("Fallo al crear una cita. El turno ya paso. Fecha del turno: {fechaCompletaTurno}", fechaCompletaTurno);
                return Result<object>.Failure("No se pueden agendar citas en un horario que ya paso.");
            }

            if (fechaCompletaTurno > DateTime.Now.AddDays(7))
            {
                _logger.LogError("Fallo al crear una cita. El turno está demasiado lejos. Fecha del turno: {fechaCompletaTurno}", fechaCompletaTurno);
                return Result<object>.Failure("Las citas solo se pueden agendar con un máximo de 7 días de antelación.");
            }

            var citasDelUsuario = await _citaRepository.GetCitasByUsuarioAsync(crearCita.IdUsuario);

            var otraCitaMismoDia = citasDelUsuario.Any(cita =>
            {
                var fechaDeOtraCita = cita.ConfiguracionTurno.FechaInicio.Date;
                return fechaDeOtraCita == turno.FechaInicio.Date;
            });

            if (otraCitaMismoDia)
            {
                _logger.LogError(
                    "Fallo al crear una cita. El usuario {IdUsuario} ya tiene otra cita agendada para este día.",
                    crearCita.IdUsuario);
                return Result<object>.Failure("Ya tienes una cita agendada para este día.");
            }
            // Contar el número de citas existentes para el turno
            var citasExistentes = (await _citaRepository.GetByFechaAsync(turno.FechaInicio)).Count();

            // Validar si el cupo está disponible
            if (citasExistentes >= turno.CantidadEstaciones)
            {
                _logger.LogError(
                    "Fallo al crear una cita. El turno con ID {TurnoId} ha alcanzado el límite de citas.",
                    crearCita.TurnoId);
                return Result<object>.Failure("No hay estaciones disponibles para este turno.");
            }

            // Si todas las validaciones pasan, la operación es un éxito.
            return Result<object>.Success(new object());
        }
    }
}
