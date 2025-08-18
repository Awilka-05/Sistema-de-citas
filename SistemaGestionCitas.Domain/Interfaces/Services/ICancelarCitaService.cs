using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Result_Pattern;

namespace SistemaGestionCitas.Domain.Interfaces.Services
{
    public interface ICancelarCitaService
    {
        Task<Result<Cita>> CancelarCitaAsync(int citaId, Usuario usuario);
    }
}