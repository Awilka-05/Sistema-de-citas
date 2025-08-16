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
        public int IdCita { get; set; }
        public int IdUsuario { get; set; }
        public int TurnoId { get; set; }
        public short LugarId { get; set; }
        public short ServicioId { get; set; }
        public EstadoCita Estado { get; set; }
    }
}
