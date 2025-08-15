using SistemaGestionCitas.Domain.Entities;

namespace SistemaGestionCitas.Domain.Interfaces.Repositories
{
    public interface ICitaRepository : IRepository<Cita>
    {
        Task<IEnumerable<Cita>> GetByFechaAsync(DateTime fecha);
        Task<bool> EstaDisponibleAsync(int idSlot);
    }
}
