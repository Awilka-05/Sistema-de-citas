using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionCitas.Application.DTOs.Responses
{
    internal class ActualizarEstadoCitaResponseDto
    {
        public int IdCita { get; set; }
        public string EstadoActual { get; set; } = null!;
        public DateTime FechaActualizacion { get; set; }
    }
}
