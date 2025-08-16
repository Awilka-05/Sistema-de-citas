using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Entities;

namespace SistemaGestionCitas.Infrastructure.Services.Correo.Strategy
{
    public interface ICorreoEstrategia
    {
        Task EnviarAsync(Cita cita, Usuario usuario);
    }
}
