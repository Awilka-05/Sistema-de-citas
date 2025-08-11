using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionCitas.Domain.Entities
{
    public class CorreoPendiente
    {
        public int IdCorreo { get; set; }
        public int IdUsuario { get; set; }
        public int? IdCita { get; set; }
        public string Destinatario { get; set; } = null!;
        public string Asunto { get; set; } = null!;
        public string Cuerpo { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public int IntentosEnvio { get; set; } = 0;
        public int MaxIntentos { get; set; } = 3;
        public bool Enviado { get; set; } = false;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaEnvio { get; set; }
        public string? ErrorMensaje { get; set; }
        public Usuario? Usuario { get; set; }
        public Cita? Cita { get; set; }
    }
}
