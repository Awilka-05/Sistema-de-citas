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
        [Required(ErrorMessage = "El estado es requerido")]
        public EstadoCita Estado { get; set; }

        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
        public string? Observaciones { get; set; }
    }
}
