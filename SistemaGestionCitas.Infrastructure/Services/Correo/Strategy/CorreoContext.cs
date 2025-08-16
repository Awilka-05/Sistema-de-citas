using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Entities;

namespace SistemaGestionCitas.Infrastructure.Services.Correo.Strategy
{
    public class CorreoContext
    {
        private ICorreoEstrategia _estrategia;

        public void SetStrategy(ICorreoEstrategia estrategia)
        {
            _estrategia = estrategia;
        }

        public async Task EjecutarAsync(Cita cita, Usuario usuario)
        {
            if (_estrategia != null)
                await _estrategia.EnviarAsync(cita, usuario);
        }
    }
}
