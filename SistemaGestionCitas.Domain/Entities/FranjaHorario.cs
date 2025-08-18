using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionCitas.Domain.Entities
{
    public class FranjaHorario
    {
        public int FranjaId { get; set; }
        public int ConfiguracionTurnoId { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }

        public ConfiguracionTurno ConfiguracionTurno { get; set; } = null!;
    }
}
