using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Interfaces.Repositories;

namespace SistemaGestionCitas.Application.Interfaces.Repositories
{
    public interface ICitaRepository : IRepository<Cita>
    {
        Task<IEnumerable<Cita>> GetByFechaAsync(DateTime fecha);
        Task<bool> EstaDisponibleAsync(int idSlot);
    }
}
