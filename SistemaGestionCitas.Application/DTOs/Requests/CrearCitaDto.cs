using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SistemaGestionCitas.Application.Validators;
using SistemaGestionCitas.Domain.Enums;

namespace SistemaGestionCitas.Application.DTOs.Requests
{
    public class CrearCitaDto
    {
      
        [IDValido(ErrorMessage = "El ID del Usuario no es válido.")]
        public int IdUsuario { get; set; }
        [Required(ErrorMessage = "La fecha de la cita es obligatoria.")]
        public DateTime FechaCita { get; set; }

        public int TurnoId { get; set; }

        [IDValido(ErrorMessage = "El ID del Lugar no es válido.")]
        public short LugarId { get; set; }

        [IDValido(ErrorMessage = "El ID del Servicio no es válido.")]
        public short ServicioId { get; set; }
        
    }
}
