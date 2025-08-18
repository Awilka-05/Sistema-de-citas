using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SistemaGestionCitas.Application.DTOs.Requests;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Domain.Interfaces.Services;
using SistemaGestionCitas.Domain.Result_Pattern;

namespace SistemaGestionCitas.Application.Services
{
    public class HorarioService : IHorarioService
    {
        private readonly IRepository<Horario, short> _horarioRepository;
        private readonly ILogger<HorarioService> _logger;

        public HorarioService(IRepository<Horario, short> repository, ILogger<HorarioService> logger)
        {
            _horarioRepository = repository;
            _logger = logger;
        }

        public async Task<Result<Horario>> GetByIdAsync(short id)
        {
            var horario = await _horarioRepository.GetByIdAsync(id);
            if (horario == null)
            {
                return Result<Horario>.Failure("Horario no encontrado.");
            }
            return Result<Horario>.Success(horario);
        }

        public async Task<Result<IEnumerable<Horario>>> GetAllAsync()
        {
            var horarios = await _horarioRepository.GetAllAsync();
            return Result<IEnumerable<Horario>>.Success(horarios);
        }

        public async Task<Result<Horario>> AddAsync(Horario entity)
        {
           if (entity == null)
            {
                _logger.LogWarning("Intento de agregar un horario nulo.");
                return Result<Horario>.Failure("El horario no puede ser nulo.");
            }
            var all = await _horarioRepository.GetAllAsync();
            if (all.Any(h => h.HoraInicio == entity.HoraInicio && h.HoraFin == entity.HoraFin))
            {
                _logger.LogWarning($"Ya existe un horario con HoraInicio '{entity.HoraInicio}' y HoraFin '{entity.HoraFin}'.");
                return Result<Horario>.Failure("El horario ya existe.");
            }
      
            await _horarioRepository.AddAsync(entity);
            _logger.LogInformation($"Horario '{entity.HorarioId}' agregado exitosamente.");
            return Result<Horario>.Success(entity);
        }
        public async Task<Result<Horario>> UpdateAsync(Horario entity)
        {
            var lugar = await _horarioRepository.GetByIdAsync(entity.HorarioId);
            if (lugar == null)
            {
                _logger.LogWarning($"Horario con ID {entity.HorarioId} no encontrado para actualizar.");
                return Result<Horario>.Failure("Lugar no encontrado.");
            }

            await _horarioRepository.UpdateAsync(entity);
            _logger.LogInformation($"Horario '{entity.Descripcion}' actualizado.");
            return Result<Horario>.Success(entity);
        }
    }
}
