using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Enums;

namespace SistemaGestionCitas.Domain.Interfaces.Repositories
{
    public interface ICitaRepository : IRepository<Cita, int>
    {
        Task<IEnumerable<Cita>> GetCitasByUsuarioAsync(int usuarioId);
        Task<IEnumerable<Cita>> GetByFechaAsync(DateTime fecha);
        Task<IEnumerable<Cita>> GetByEstadoAsync(EstadoCita estado);
    }
   
}
