
using System.ComponentModel.DataAnnotations;

using SistemaGestionCitas.Domain.Enums;

namespace SistemaGestionCitas.Application.DTOs.Requests
{
    public class ActualizarEstadoCitaDto
    {
        [Required(ErrorMessage = "El estado es requerido")]
        public EstadoCita Estado { get; set; }

    }
}
