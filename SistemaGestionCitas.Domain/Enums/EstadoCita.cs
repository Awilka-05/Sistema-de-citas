using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionCitas.Domain.Enums
{
    public enum EstadoCita
    {
        Pendiente = 0,
        Confirmada = 1,
        Cancelada = 2,
        Completada = 3,
        NoAsistio = 4
    }
}

