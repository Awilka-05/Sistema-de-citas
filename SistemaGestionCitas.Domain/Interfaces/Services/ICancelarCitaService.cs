using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionCitas.Domain.Interfaces.Services
{
    public interface ICancelarCitaService
    {
        Task CancelarCitaAsync(int citaId, int usuarioId);
    }
}
