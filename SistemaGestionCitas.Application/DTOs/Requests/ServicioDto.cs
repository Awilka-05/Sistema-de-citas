
using System.ComponentModel.DataAnnotations;


namespace SistemaGestionCitas.Application.DTOs.Requests
{
    public class ServicioDto
    {
        [Required(ErrorMessage = "El nombre del servicio es necesario")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El precio del servicio es necesario")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La duración del servicio es necesaria")]
        public int DuracionMinutos { get; set; } 
    }
}
