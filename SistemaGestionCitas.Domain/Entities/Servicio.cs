using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionCitas.Domain.Entities
{
    public class Servicio
    {
        public short ServicioId { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }

        public ICollection<Cita> Citas { get; set; } = null!;
    }
}