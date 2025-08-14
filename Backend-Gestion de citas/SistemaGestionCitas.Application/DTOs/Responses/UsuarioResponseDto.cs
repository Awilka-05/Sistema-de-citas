using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Enums;

namespace SistemaGestionCitas.Application.DTOs.Responses
{
    public class UsuarioResponseDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Cedula { get; set; } = null!;

    }
}
