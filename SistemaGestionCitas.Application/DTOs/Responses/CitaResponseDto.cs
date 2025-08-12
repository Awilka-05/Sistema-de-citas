using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Enums;

namespace SistemaGestionCitas.Application.DTOs.Responses
{
    public class CitaResponseDto
    {
        public int IdUsuario { get; set; }
        public int IdSlot { get; set; }
        public string CodigoCita { get; set; } = null!;
        public EstadoCita Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string Observaciones { get; set; } = null!;
        public UsuarioResponseDto Usuario { get; set; } = null!;
        public SlotHorarioResponseDto SlotHorario { get; set; } = null!;
    }
}
