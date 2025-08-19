using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Enums;
using SistemaGestionCitas.Domain.Value_Objects;

namespace SistemaGestionCitas.Application.DTOs.Responses
{
    public class UsuarioResponseDto
    {
        public int IdUsuario { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public required string Nombre { get; set; }
        public required string Cedula { get; set; }
        public required string Correo { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public RolUsuario Rol { get; set; }

    }
}
