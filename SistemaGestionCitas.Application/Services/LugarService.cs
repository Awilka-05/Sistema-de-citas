using Microsoft.Extensions.Logging;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Domain.Interfaces.Services;
using SistemaGestionCitas.Domain.Result_Pattern;

namespace SistemaGestionCitas.Application.Services
{
    public class LugarService : ILugarService
    {
        private readonly IRepository<Lugar,short> _repository;
        private readonly ILogger<LugarService> _logger;

        public LugarService(IRepository<Lugar,short> repository, ILogger<LugarService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<Lugar>> GetByIdAsync(short id)
        {
            var lugar = await _repository.GetByIdAsync(id);
            if (lugar == null)
            {
                _logger.LogWarning($"Lugar con ID {id} no encontrado.");
                return Result<Lugar>.Failure("Lugar no encontrado.");
            }

            return Result<Lugar>.Success(lugar);
        }

        public async Task<Result<IEnumerable<Lugar>>> GetAllAsync()
        {
            var lugares = await _repository.GetAllAsync();
            return Result<IEnumerable<Lugar>>.Success(lugares);
        }

        public async Task<Result<Lugar>> AddAsync(Lugar entity)
        {
            var all = await _repository.GetAllAsync();
            if (all.Any(l => l.Nombre.ToLower() == entity.Nombre.ToLower()))
            {
                _logger.LogWarning($"Ya existe un lugar con el nombre '{entity.Nombre}'.");
                return Result<Lugar>.Failure("El lugar ya existe.");
            }

            await _repository.AddAsync(entity);
            _logger.LogInformation($"Lugar '{entity.Nombre}' agregado exitosamente.");
            return Result<Lugar>.Success(entity);
        }

        public async Task<Result<Lugar>> UpdateAsync(Lugar entity)
        {
            var lugar = await _repository.GetByIdAsync(entity.LugarId);
            if (lugar == null)
            {
                _logger.LogWarning($"Lugar con ID {entity.LugarId} no encontrado para actualizar.");
                return Result<Lugar>.Failure("Lugar no encontrado.");
            }

            await _repository.UpdateAsync(entity);
            _logger.LogInformation($"Lugar '{entity.Nombre}' actualizado.");
            return Result<Lugar>.Success(entity);
        }
    }

}
