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
        public string Estado { get; set; } = null!;

        public UsuarioResponseDto Usuario { get; set; } = null!;
        public ConfiguracionTurnoResponseDto ConfiguracionTurno { get; set; } = null!;
        public LugarResponseDto Lugar { get; set; } = null!;
        public ServicioResponseDto Servicio { get; set; } = null!;
    }
}
