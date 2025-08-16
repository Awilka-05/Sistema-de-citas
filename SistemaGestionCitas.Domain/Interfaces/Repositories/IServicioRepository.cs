using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Entities;

namespace SistemaGestionCitas.Domain.Interfaces.Repositories
{
    public interface IServicioRepository : IRepository<Servicio,short>
    {
    }
}
