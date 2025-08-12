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
        public string Nombre { get; set; }

        [MaxLength(255, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La duración del servicio es necesaria")]
        public int DuracionMinutos { get; set; } 
    }
}
