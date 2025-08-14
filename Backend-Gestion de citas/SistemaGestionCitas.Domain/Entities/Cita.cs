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
        public int TurnoId { get; set; }
        public short LugarId { get; set; }
        public short ServicioId { get; set; }
        public EstadoCita Estado { get; set; } 
        public byte[] RowVersion { get; set; } = null!;

        public Usuario Usuario { get; set; } = null!;
        public ConfiguracionTurno ConfiguracionTurno { get; set; } = null!;
        public Lugar Lugar { get; set; } = null!;
        public Servicio Servicio { get; set; } = null!;
    }
}
