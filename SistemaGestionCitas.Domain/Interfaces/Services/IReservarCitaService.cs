using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Result_Pattern;

namespace SistemaGestionCitas.Domain.Interfaces.Services
{
    public interface IReservarCitaService
    {
        Task<Result<Cita>> ReservarCitaAsync(Cita cita);
    }
}

