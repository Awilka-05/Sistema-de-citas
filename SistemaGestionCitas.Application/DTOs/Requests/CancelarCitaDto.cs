using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionCitas.Application.DTOs.Requests
{
   public class CancelarCitaDto
    {
        [Required(ErrorMessage = "El id de la cita es requerido")]
        public int IdCita { get; set; }
    }
}
