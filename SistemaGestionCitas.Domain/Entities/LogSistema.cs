using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Enums;

namespace SistemaGestionCitas.Domain.Entities
{
    public class LogSistema
    {
        public int IdLog { get; set; }
        public int? IdUsuario { get; set; }
        public string Accion { get; set; } = null!;
        public string? Detalle { get; set; }
        public TipoLog Tipo { get; set; }
        public string? DireccionIp { get; set; }
        public string? UserAgent { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public Usuario? Usuario { get; set; }
    }
}
