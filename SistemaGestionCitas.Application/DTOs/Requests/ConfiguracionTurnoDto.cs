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
        [Required(ErrorMessage = "La hora de inicio es requerida")]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "La hora de fin es requerida")]
        public TimeSpan HoraFin { get; set; }

        [Required(ErrorMessage = "La duración del slot es requerida")]
        public int DuracionSlotMinutos { get; set; }
        public bool Activo { get; set; }
    }
}
