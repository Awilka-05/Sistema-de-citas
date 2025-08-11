using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionCitas.Domain.Entities
{
    public class AuditoriaCita
    {
        public int IdAuditoria { get; set; }
        public int IdCita { get; set; }
        public string? EstadoAnterior { get; set; }
        public string EstadoNuevo { get; set; } = null!;
        public int? IdUsuarioCambio { get; set; }
        public string? Motivo { get; set; }
        public DateTime FechaCambio { get; set; } = DateTime.Now;
        public Cita? Cita { get; set; }
        public Usuario? UsuarioCambio { get; set; }
    }
}
