using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaGestionCitas.Application.DTOs.Responses;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Domain.Interfaces.Services;
using SistemaGestionCitas.Domain.Result_Pattern;
using SistemaGestionCitas.Infrastructure.Persistence.BdContext;
using SistemaGestionCitas.Infrastructure.Repositories;

namespace SistemaGestionCitas.Application.Services
{
    public class ConfiguracionTurnoService : IConfiguracionTurnoService
    {
        private readonly IRepository<ConfiguracionTurno, int> _turnoRepository;
        private readonly SistemaCitasDbContext _context;
        private readonly ILogger<ConfiguracionTurnoService> _logger;

        public ConfiguracionTurnoService(IRepository<ConfiguracionTurno, int> repository, SistemaCitasDbContext context, ILogger<ConfiguracionTurnoService> logger)
        {
            _turnoRepository = repository;
            _context = context;
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

            bool Yahayenesafecha = await _context.ConfiguracionesTurnos
           .AnyAsync(ct => entity.FechaInicio <= ct.FechaFin &&
                        entity.FechaFin >= ct.FechaInicio);

            if (Yahayenesafecha)
                return Result<ConfiguracionTurno>.Failure("Ya existe una configuración de turno en esa fecha");

            // Validar que el horario exista
            var horarioBase = await _context.Horarios.FindAsync(entity.HorariosId);
            if (horarioBase == null)
                return Result<ConfiguracionTurno>.Failure("El horario seleccionado no existe.");

            entity.Horario = horarioBase;
            entity.Franjas = GenerarFranjas(horarioBase.HoraInicio, horarioBase.HoraFin, entity.DuracionMinutos);

            await _turnoRepository.AddAsync(entity);

            return Result<ConfiguracionTurno>.Success(entity);
        }

        // Método privado para generar franjas
        private List<FranjaHorario> GenerarFranjas(TimeOnly horaInicio, TimeOnly horaFin, int duracionMinutos)
    {
        var franjas = new List<FranjaHorario>();
        var actual = horaInicio;
        int numero = 1;

        while (actual.AddMinutes(duracionMinutos) <= horaFin)
        {
            franjas.Add(new FranjaHorario
            {
                HoraInicio = actual,
                HoraFin = actual.AddMinutes(duracionMinutos)
            });
            actual = actual.AddMinutes(duracionMinutos);
            numero++;
        }

        return franjas;
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
