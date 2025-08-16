
using System.ComponentModel.DataAnnotations;


namespace SistemaGestionCitas.Application.DTOs.Requests
{
    public class ServicioDto
    {
        [Required(ErrorMessage = "El nombre del servicio es necesario")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El nombre del servicio debe tener entre 5 y 100 caracteres.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El precio del servicio es necesario")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La duración del servicio es necesaria")]
        public int DuracionMinutos { get; set; } 
    }
}
