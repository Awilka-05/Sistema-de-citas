
using Microsoft.Extensions.Logging;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Domain.Interfaces.Services;
using SistemaGestionCitas.Domain.Result_Pattern;

namespace SistemaGestionCitas.Application.Services
{
    public class ServicioService : IServicioService
    {
        private readonly IRepository<Servicio, short> _repository;
        private readonly ILogger<ServicioService> _logger;

        public ServicioService(IRepository<Servicio, short> repository, ILogger<ServicioService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<Servicio>> GetByIdAsync(short id)
        {
            var Servicio = await _repository.GetByIdAsync(id);
            if (Servicio == null)
            {
                _logger.LogWarning($"Servicio con ID {id} no encontrado.");
                return Result<Servicio>.Failure("Servicio no encontrado.");
            }

            return Result<Servicio>.Success(Servicio);
        }

        public async Task<Result<IEnumerable<Servicio>>> GetAllAsync()
        {
            var servicios = await _repository.GetAllAsync();
            return Result<IEnumerable<Servicio>>.Success(servicios);
        }

        public async Task<Result<Servicio>> AddAsync(Servicio entity)
        {
            var all = await _repository.GetAllAsync();
            if (all.Any(l => l.Nombre.ToLower() == entity.Nombre.ToLower()))
            {
                _logger.LogWarning($"Ya existe un Servicio con el nombre '{entity.Nombre}'.");
                return Result<Servicio>.Failure("El Servicio ya existe.");
            }

            await _repository.AddAsync(entity);
            _logger.LogInformation($"Servicio '{entity.Nombre}' agregado exitosamente.");
            return Result<Servicio>.Success(entity);
        }

        public async Task<Result<Servicio>> UpdateAsync(Servicio entity)
        {
            var servicio = await _repository.GetByIdAsync(entity.ServicioId);
            if (servicio == null)
            {
                _logger.LogWarning($"Servicio con ID {entity.ServicioId} no encontrado");
                return Result<Servicio>.Failure("Servicio no encontrado.");
            }

            await _repository.UpdateAsync(entity);
            _logger.LogInformation($"Servicio '{entity.Nombre}' actualizado.");
            return Result<Servicio>.Success(entity);
        }

        public async Task<Result<bool>> DeleteAsync(short id)
        {
            var Servicio = await _repository.GetByIdAsync(id);
            if (Servicio == null)
            {
                _logger.LogWarning($"Servicio con ID {id} no encontrado");
                return Result<bool>.Failure("Servicio no encontrado.");
            }

            await _repository.DeleteAsync(id);
            _logger.LogInformation($"Servicio con ID {id} eliminado.");
            return Result<bool>.Success(true);
        }
    }
}
