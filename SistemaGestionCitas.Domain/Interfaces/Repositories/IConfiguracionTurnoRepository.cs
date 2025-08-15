using SistemaGestionCitas.Domain.Entities;

namespace SistemaGestionCitas.Domain.Interfaces.Repositories;

public interface IConfiguracionTurnoRepository : IRepository<ConfiguracionTurno>
{
    Task<List<ConfiguracionTurno>> GetTurnosActivosAsync();
    Task<List<ConfiguracionTurno>> GetTurnosPorFechaAsync(DateTime fecha);
    Task<ConfiguracionTurno?> GetTurnoPorHorarioAsync(short id);
    Task<ConfiguracionTurno?> GetTurnoConDetallesAsync(int turnoId);
    Task<List<ConfiguracionTurno>> GetTurnosDisponiblesAsync(DateTime fechaInicio, DateTime fechaFin);
}