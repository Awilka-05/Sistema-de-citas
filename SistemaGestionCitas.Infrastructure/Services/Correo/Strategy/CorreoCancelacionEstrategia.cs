using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Entities;

namespace SistemaGestionCitas.Infrastructure.Services.Correo.Strategy
{
    public class CorreoCancelacionEstrategia : ICorreoEstrategia
    {
        public async Task EnviarAsync(Cita cita, Usuario usuario)
        {
            string template = await File.ReadAllTextAsync("C:\\SistemaDeGestionDeCitas\\SistemaGestionCitas.Infrastructure\\Services\\Correo\\Plantillas\\cancelacion.html");
            string body = template
                .Replace("{{UserName}}", usuario.Nombre.Value)
                .Replace("{{IdCita}}", cita.IdCita.ToString())
                .Replace("{{Servicio}}", cita.Servicio.Nombre)
                .Replace("{{Precio}}", cita.Servicio.Precio.ToString())
                .Replace("{{Horario}}", cita.ConfiguracionTurno.HorariosId.ToString())
                .Replace("{{Duracion}}", cita.ConfiguracionTurno.DuracionMinutos.ToString())
                .Replace("{{Lugar}}", cita.Lugar.Nombre);

            await CorreoSender.EnviarAsync(usuario.Correo.Value, "Cancelación de cita", body);
        }
        
    }
}
