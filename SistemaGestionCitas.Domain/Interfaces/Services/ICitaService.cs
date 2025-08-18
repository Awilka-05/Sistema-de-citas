using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Enums;
using SistemaGestionCitas.Domain.Result_Pattern;

namespace SistemaGestionCitas.Domain.Interfaces.Services
{
    public interface ICitaService
    {
        Task<Result<Cita>> GetByIdAsync(int id);
        Task<Result<IEnumerable<Cita>>> GetAllAsync();
        Task<Result<IEnumerable<Cita>>> GetByFechaAsync(DateTime fecha);
        Task<Result<IEnumerable<Cita>>> GetByUsuarioAsync(int usuarioId);
        Task<IEnumerable<Cita>> GetByEstadoAsync(EstadoCita estado);

    }
}
