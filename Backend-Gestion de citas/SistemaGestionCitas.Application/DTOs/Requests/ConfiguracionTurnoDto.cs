using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionCitas.Application.DTOs.Requests
{
    public class ConfiguracionTurnoDto
    {
        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es requerida")]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "El ID del horario es requerido")]
        public short HorariosId { get; set; }

        [Required(ErrorMessage = "La duración de estaciones es requerida")]
        public int CantidadEstaciones { get; set; }

        [Required(ErrorMessage = "La duración del slot es requerida")]
        public int DuracionMinutos { get; set; }
        
    }
}
