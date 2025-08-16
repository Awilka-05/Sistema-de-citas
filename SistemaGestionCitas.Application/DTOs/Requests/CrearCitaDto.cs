using System.ComponentModel.DataAnnotations;
using SistemaGestionCitas.Application.Validators;
using SistemaGestionCitas.Domain.Enums;

namespace SistemaGestionCitas.Application.DTOs.Requests
{
    public class CrearCitaDto
    {
      
        [IDValido(ErrorMessage = "El ID del Usuario no es válido.")]
        public int IdUsuario { get; set; }

        [IDValido(ErrorMessage = "El ID del Turno no es válido.")]
        public int TurnoId { get; set; }

        [IDValido(ErrorMessage = "El ID del Lugar no es válido.")]
        public short LugarId { get; set; }

        [IDValido(ErrorMessage = "El ID del Servicio no es válido.")]
        public short ServicioId { get; set; }
        
    }
}
