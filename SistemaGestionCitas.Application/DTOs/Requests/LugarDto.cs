
using System.ComponentModel.DataAnnotations; 

namespace SistemaGestionCitas.Application.DTOs.Requests
{
    public class LugarDto
    {
        [Required (ErrorMessage = "El nombre del lugar es requerido")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El nombre del lugar debe tener entre 5 y 100 caracteres.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre del lugar solo puede contener letras y espacios.")]
        public string Nombre { get; set; }
    }
}
