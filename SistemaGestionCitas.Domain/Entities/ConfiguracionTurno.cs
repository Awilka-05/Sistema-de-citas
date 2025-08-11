using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Enums;

namespace SistemaGestionCitas.Domain.Entities
{
   public class ConfiguracionTurno
    {
        public int IdConfiguracion { get; set; }
        public DateTime Fecha { get; set; }
        public TipoTurno Turno { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public int IdServicio { get; set; }
        public int EstacionesDisponibles { get; set; }
        public int DuracionSlotMinutos { get; set; } = 15;
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public Servicio? Servicio { get; set; }
        public ICollection<SlotHorario>? SlotsHorario { get; set; }

    }
}
