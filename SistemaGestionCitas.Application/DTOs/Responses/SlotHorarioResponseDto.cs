using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionCitas.Application.DTOs.Responses
{
    public class SlotHorarioResponseDto
    {
        public int IdSlot { get; set; }
        public DateTime FechaHora { get; set; }
        public int DuracionMinutos { get; set; }
        public int IdServicio { get; set; }
        public bool Disponible { get; set; }
        public string Observaciones { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        public ServicioResponseDto Servicio { get; set; } = null!;
    }
}
