using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Enums;

namespace SistemaGestionCitas.Application.DTOs.Requests
{
    public class ActualizarEstadoCitaDto
    {
        [Required(ErrorMessage = "El id de la cita es requerido")]
        public int IdCita { get; set; }
        [Required(ErrorMessage = "El estado es requerido")]
        public string NuevaEstado { get; set; } = null!;

    }
}
