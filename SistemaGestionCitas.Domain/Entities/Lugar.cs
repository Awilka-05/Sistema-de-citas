
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionCitas.Domain.Entities
{
    public class Lugar
    {
        public short Id { get; set; }
        public string Nombre { get; set; } = null!;

        public ICollection<Cita> Citas { get; set; } = null!;
    }
}
