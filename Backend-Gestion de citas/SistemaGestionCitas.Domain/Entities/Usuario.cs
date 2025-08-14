using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Enums;

namespace SistemaGestionCitas.Domain.Entities
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Nombre { get; set; } =null!;  
        public string Cedula { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Contrasena { get; set; } = null!;
        public RolUsuario Rol { get; set; } 
        public bool Activo { get; set; }

        public ICollection<Cita> Citas { get; set; } =null!;
    }
}
