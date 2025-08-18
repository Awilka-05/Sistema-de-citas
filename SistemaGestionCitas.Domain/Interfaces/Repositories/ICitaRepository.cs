using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Enums;

namespace SistemaGestionCitas.Domain.Interfaces.Repositories
{
    public interface ICitaRepository 
    {
        Task<Cita?> GetByIdAsync(int id);
        Task<IEnumerable<Cita>> GetAllAsync();
        Task AddAsync(Cita entity);
        Task UpdateAsync(Cita entity); 
        Task<IEnumerable<Cita>> GetCitasByUsuarioAsync(int usuarioId);
        Task<IEnumerable<Cita>> GetByFechaAsync(DateTime fecha);
        Task<IEnumerable<Cita>> GetByEstadoAsync(EstadoCita estado);
    }
   
}
