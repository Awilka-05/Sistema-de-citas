using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Entities;

namespace SistemaGestionCitas.Infrastructure.Services.Correo.Strategy
{
    public class CorreoConfirmacionEstrategia : ICorreoEstrategia
    {
        public async Task EnviarAsync(Cita cita, Usuario usuario, FranjaHorario franjaSeleccionada)
        {
            string template = await File.ReadAllTextAsync("C:\\SistemaDeGestionDeCitas\\SistemaGestionCitas.Infrastructure\\Services\\Correo\\Plantillas\\confirmacion.html");
            string body = template
                .Replace("{{UserName}}", usuario.Nombre.Value)
                .Replace("{{IdCita}}", cita.IdCita.ToString())
                .Replace("{{Fecha}}", cita.FechaCita.ToString("dd/MM/yyyy"))
                .Replace("{{Servicio}}", cita.Servicio.Nombre)
                .Replace("{{Precio}}", cita.Servicio.Precio.ToString())
                .Replace("{{HorarioInicio}}", franjaSeleccionada.HoraInicio.ToString("HH:mm"))
                .Replace("{{HorarioFin}}", franjaSeleccionada.HoraFin.ToString("HH:mm"))
                .Replace("{{Duracion}}", cita.ConfiguracionTurno.DuracionMinutos.ToString()) 
                .Replace("{{Lugar}}", cita.Lugar.Nombre); 

            await CorreoSender.EnviarAsync(usuario.Correo.Value, "Confirmación de cita", body); 
        }
    }

}
