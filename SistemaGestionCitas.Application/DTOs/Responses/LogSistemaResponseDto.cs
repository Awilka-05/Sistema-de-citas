using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionCitas.Application.DTOs.Responses
{
    public class LogSistemaResponseDto
    {
        public int IdLog { get; set; }
        public string Usuario { get; set; } = null!;
        public string Accion { get; set; } = null!;
        public string Detalles { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public string DireccionIP { get; set; } = null!;
    }
}
