using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionCitas.Domain.Entities
{
    public class SlotHorario
    {
        public int IdSlot { get; set; }
        public int IdConfiguracion { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public int CupoMaximo { get; set; }
        public int CupoOcupado { get; set; } = 0;
        public bool Activo { get; set; } = true;
        public ConfiguracionTurno? ConfiguracionTurno { get; set; }
        public ICollection<Cita>? Citas { get; set; }
    }
}
