using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionCitas.Domain.Enums
{
    public enum TipoCorreo
    {
        Confirmacion = 1,
        Cancelacion = 2,
        Recordatorio = 3,
        CambioEstado
    }
}
