using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SistemaGestionCitas.Application.Validators;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Enums;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Domain.Interfaces.Services;
using SistemaGestionCitas.Domain.Result_Pattern;

namespace SistemaGestionCitas.Application.Services
{
    public class CitaService : ICitaService
    {
        private readonly ICitaRepository _citaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<CitaService> _logger;
        public CitaService(ICitaRepository citaRepository, IUsuarioRepository usuarioRepository, ILogger<CitaService> logger)
        {
            _citaRepository = citaRepository;
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }
        public async Task<Result<Cita>> GetByIdAsync(int id)
        {
            var cita = await _citaRepository.GetByIdAsync(id);
            if (cita == null)
            {
                _logger.LogError("Cita con ID {Id} no encontrada.", id);
                return Result<Cita>.Failure("Cita no encontrada.");
            }
            return Result<Cita>.Success(cita);
        }
        public async Task<Result<IEnumerable<Cita>>> GetAllAsync()
        {
            var citas = await _citaRepository.GetAllAsync();
            return Result<IEnumerable<Cita>>.Success(citas);
        }

       public async Task<Result<IEnumerable<Cita>>> GetByFechaAsync(DateTime fecha){
            var citas = await _citaRepository.GetByFechaAsync(fecha);
            return Result<IEnumerable<Cita>>.Success(citas);

        }
        public async Task<Result<IEnumerable<Cita>>> GetByUsuarioAsync(int usuarioId)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);

            if (usuario == null)
            {
                _logger.LogWarning("No se pueden obtener citas: el usuario con ID {UsuarioId} no fue encontrado.", usuarioId);
                return Result<IEnumerable<Cita>>.Failure("Usuario no encontrado.");
            }

            var citas = await _citaRepository.GetCitasByUsuarioAsync(usuarioId);
            return Result<IEnumerable<Cita>>.Success(citas);
        }
        public async Task<IEnumerable<Cita>> GetByEstadoAsync(EstadoCita estado) { 
            var citas = await _citaRepository.GetByEstadoAsync(estado);
            return citas;
        }

    }
}
