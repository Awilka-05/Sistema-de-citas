using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SistemaGestionCitas.Infrastructure.Services.Correo.Strategy
{
    public static class CorreoSender
    {
    
            private static IConfiguration _configuration;

            public static void SetConfiguration(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public static async Task EnviarAsync(string destinatario, string asunto, string cuerpoHtml)
            {
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var port = int.Parse(_configuration["EmailSettings:Port"]);
                var senderEmail = _configuration["EmailSettings:SenderEmail"];
                var appPassword = _configuration["EmailSettings:AppPassword"];

                using var smtp = new SmtpClient(smtpServer)
                {
                    Port = port,
                    Credentials = new NetworkCredential(senderEmail, appPassword),
                    EnableSsl = true
                };

                using var mensaje = new MailMessage(senderEmail, destinatario, asunto, cuerpoHtml)
                {
                    IsBodyHtml = true
                };

                await smtp.SendMailAsync(mensaje);
            }
     }

    
}
