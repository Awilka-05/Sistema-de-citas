using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionCitas.Domain.Entities
{
    public class Servicio
    {
        public int IdServicio { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public int DuracionMinutos { get; set; } = 15;
        public bool Activo { get; set; } = true;
        public ICollection<ConfiguracionTurno> ConfiguracionesTurno { get; set; } = null!;
    }
}
