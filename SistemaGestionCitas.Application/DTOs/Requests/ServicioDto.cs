using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
