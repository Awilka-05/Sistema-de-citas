using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionCitas.Application.DTOs.Responses
{
    public class CancelarCitaResponseDto
    {
        public int IdCita { get; set; }
        public string EstadoActual { get; set; } = null!;
        public DateTime FechaCancelacion { get; set; }
        public string Mensaje { get; set; } = null!;
    }
}
