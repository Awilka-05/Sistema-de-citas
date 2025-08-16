
using System.ComponentModel.DataAnnotations;
using SistemaGestionCitas.Application.Validators;


namespace SistemaGestionCitas.Application.DTOs.Requests
{
    public class ConfiguracionTurnoDto
    {
        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es válido")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es requerida")]
        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha no es válido")]
        [FechaValida("FechaInicio", ErrorMessage = "La fecha de finalización debe ser posterior a la fecha de inicio.")]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "La duración del slot es requerida")]
        [Range(15, 59, ErrorMessage = "La Duracion no es valida")]
        public int DuracionMinutos { get; set; }

        [Required(ErrorMessage = "El ID del horario es requerido")]
        public short HorariosId { get; set; }

        [Required(ErrorMessage = "La duración de estaciones es requerida")]
        [Range(1, 100, ErrorMessage = "La capacidad debe ser un número entre 1 y 100.")]
        public int CantidadEstaciones { get; set; }
        
    }
}
