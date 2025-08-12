using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionCitas.Application.DTOs.Requests
{
    public class SlotHorarioDto
    {
        [Required(ErrorMessage = "La fecha y hora son requeridas")]
        public DateTime FechaHora { get; set; }

        public int DuracionMinutos { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El ID del servicio debe ser mayor a 0")]
        public int IdServicio { get; set; }

        public bool Disponible { get; set; }

        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder los 500 caracteres")]
        public string Observaciones { get; set; } = null!;
    }
}
