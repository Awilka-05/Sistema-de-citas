
using System.ComponentModel.DataAnnotations;

namespace SistemaGestionCitas.Application.DTOs.Requests
{
    public class HorarioDto
    {
        [Required(ErrorMessage = "La hora de inicio es necesario")]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "La hora final es necesario")]
        public TimeSpan HoraFin { get; set; }

        [StringLength(200, ErrorMessage = "La descripcion no puede exceder los 200 caracteres")]
        public string Descripcion { get; set; } = null!;
    }
}
