using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Domain.Interfaces.Services;
using SistemaGestionCitas.Domain.Result_Pattern;

namespace SistemaGestionCitas.Application.Services
{
    public class ConfiguracionTurnoService : IConfiguracionTurnoService
    {
        private readonly IRepository<ConfiguracionTurno, int> _turnoRepository;
        private readonly ILogger<ConfiguracionTurnoService> _logger;

        public ConfiguracionTurnoService(IRepository<ConfiguracionTurno, int> repository, ILogger<ConfiguracionTurnoService> logger)
        {
            _turnoRepository = repository;
            _logger = logger;
        }

        public async Task<Result<ConfiguracionTurno>> GetByIdAsync(int id)
        {
            var turno = await _turnoRepository.GetByIdAsync(id);
            if (turno == null)
            {
                return Result<ConfiguracionTurno>.Failure("Configuración de turno no encontrada.");
            }
            return Result<ConfiguracionTurno>.Success(turno);
        }
        public async Task<Result<IEnumerable<ConfiguracionTurno>>> GetAllAsync()
        {
            var turnos = await _turnoRepository.GetAllAsync();
            return Result<IEnumerable<ConfiguracionTurno>>.Success(turnos);
        }
        public async Task<Result<ConfiguracionTurno>> AddAsync(ConfiguracionTurno entity)
        {
            if (entity == null)
            {
                _logger.LogWarning("Intento de agregar una configuración de turno nula.");
                return Result<ConfiguracionTurno>.Failure("La configuración de turno no puede ser nula.");
            }
            var all = await _turnoRepository.GetAllAsync();
            await _turnoRepository.AddAsync(entity);
            _logger.LogInformation($"Configuración de turno '{entity.TurnoId}' agregada exitosamente.");
            return Result<ConfiguracionTurno>.Success(entity);
        }
        public async Task<Result<ConfiguracionTurno>> UpdateAsync(ConfiguracionTurno entity)
        {
            var turno = await _turnoRepository.GetByIdAsync(entity.TurnoId);
            if (turno == null)
            {
                _logger.LogWarning($"Configuración de turno con ID {entity.TurnoId} no encontrada.");
                return Result<ConfiguracionTurno>.Failure("Configuración de turno no encontrada.");
            }
            await _turnoRepository.UpdateAsync(entity);
            _logger.LogInformation($"Configuración de turno '{entity.TurnoId}' actualizada exitosamente.");
            return Result<ConfiguracionTurno>.Success(entity);
        }
    }
}
