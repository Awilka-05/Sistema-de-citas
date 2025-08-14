using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Enums;

namespace SistemaGestionCitas.Application.DTOs.Requests
{
    public class CrearUsuarioDto
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        public string Nombre { get; set; } = null!;
        [Required(ErrorMessage = "La fecha de nacimiento es necesaria")]
        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es válido")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [StringLength(200, ErrorMessage = "El email no puede exceder los 200 caracteres")]
        public string Correo { get; set; } = null!;

        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres")]
        public string Telefono { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
        public string Contrasena { get; set; } = null!;

        [Required(ErrorMessage = "La cédula es requerida")]
        [StringLength(11, ErrorMessage = "La cédula no puede exceder los 11 caracteres")]
        public string Cedula { get; set; } = null!;

        [Required(ErrorMessage = "El rol es requerido")]
        public RolUsuario Rol { get; set; }
    }
}
