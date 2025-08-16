using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Interfaces.Services;

namespace SistemaGestionCitas.Application.UseCases
{
    public class CancelarCita : ICancelarCitaService
    {
        private readonly ICancelarCitaService _cancelarCitaService;
        public CancelarCita(ICancelarCitaService cancelarCitaService)
        {
            _cancelarCitaService = cancelarCitaService;
        }
        public async Task<bool> CancelarAsync(int idCita, byte[] rowVersion)
        {
            return await _cancelarCitaService.CancelarAsync(idCita, rowVersion);
        }
    }
}
