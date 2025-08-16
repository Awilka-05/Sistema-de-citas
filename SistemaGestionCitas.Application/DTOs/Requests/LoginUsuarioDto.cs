
using System.ComponentModel.DataAnnotations;

namespace SistemaGestionCitas.Application.DTOs.Requests
{
    public class LoginUsuarioDto
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Contrasena { get; set; } = null!;
    }
}
