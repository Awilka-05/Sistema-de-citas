using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionCitas.Application.DTOs.Responses
{
    public class HorarioResponseDto
    {
        public short HorarioId { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string Descripcion { get; set; } = null!;
    }
}
