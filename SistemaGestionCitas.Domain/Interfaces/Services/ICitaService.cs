using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Enums;

namespace SistemaGestionCitas.Domain.Interfaces.Services
{
    public interface ICitaService
    {
        Task<IEnumerable<Cita>> GetAllAsync();
        Task<Cita?> GetByIdAsync(int id);
        Task<IEnumerable<Cita>> GetByEstadoAsync(EstadoCita estado);
    }
}