using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Enums;
using SistemaGestionCitas.Domain.Value_Objects;

namespace SistemaGestionCitas.Domain.Entities
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public required Nombre Nombre { get; set; }
        public required Cedula Cedula { get; set; }
        public required Correo Correo { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Contrasena { get; set; } = null!;
        public RolUsuario Rol { get; set; }
        public bool Activo { get; set; }

        public ICollection<Cita> Citas { get; set; } 

        public Usuario(Nombre nombre, Cedula cedula, Correo correo,
                 DateTime fechaNacimiento, string telefono, string contrasena, RolUsuario rol)
        {
            Nombre = nombre;
            Cedula = cedula;
            Correo = correo;
            FechaNacimiento = fechaNacimiento;
            Telefono = telefono;
            Contrasena = contrasena;
            Rol = rol;
            Activo = true;
            Citas = new List<Cita>();
        }
        // Constructor vacío para EF Core, ya no es necesario
        public Usuario() { }
    }
}
