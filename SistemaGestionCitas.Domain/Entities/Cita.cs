using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Enums;

namespace SistemaGestionCitas.Domain.Entities
{
    public class Cita
    {
        public int IdCita { get; set; }
        public int IdUsuario { get; set; }
        public int IdSlot { get; set; }
        public string CodigoCita { get; set; } = null!;
        public EstadoCita Estado { get; set; } = EstadoCita.Pendiente;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime FechaActualizacion { get; set; } = DateTime.Now;
        public string? Observaciones { get; set; }

      
        public Usuario? Usuario { get; set; }
        public SlotHorario? SlotHorario { get; set; }
        public ICollection<CorreoPendiente>? CorreosPendientes { get; set; }
        public ICollection<AuditoriaCita>? AuditoriasEstado { get; set; }
    }
}
