using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Enums;

namespace SistemaGestionCitas.Application.DTOs.Requests
{
    public class CrearCitaDto
    {
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        public int IdUsuario { get; set; }
        [Required(ErrorMessage = "El ID del turno es requerido")]
        public int TurnoId { get; set; }
        [Required(ErrorMessage = "El ID del lugar es requerido")]
        public short LugarId { get; set; }
        [Required(ErrorMessage = "El ID del servicio es requerido")]
        public short ServicioId { get; set; }
        [Required(ErrorMessage = "El estado de la cita es necesario")]
        public EstadoCita Estado { get; set; }

        [Required(ErrorMessage = "El ID del slot es requerido")]
        public int IdSlot { get; set; }

    }
}
