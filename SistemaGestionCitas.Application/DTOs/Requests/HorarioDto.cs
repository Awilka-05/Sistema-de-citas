
using System.ComponentModel.DataAnnotations;

namespace SistemaGestionCitas.Application.DTOs.Requests
{
    public class HorarioDto
    {
        [Required(ErrorMessage = "La hora de inicio es necesario")]
        public TimeOnly HoraInicio { get; set; }

        [Required(ErrorMessage = "La hora final es necesario")]
        public TimeOnly HoraFin { get; set; }

        [StringLength(100, MinimumLength = 5, ErrorMessage = "La descripcion debe tener entre 5 y 100 caracteres.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "La descripcion solo puede contener letras y espacios.")]
        public string Descripcion { get; set; } = null!;
    }
}
