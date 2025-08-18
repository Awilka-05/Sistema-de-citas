using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Result_Pattern;
using SistemaGestionCitas.Infrastructure.Services.Correo.Strategy;

namespace SistemaGestionCitas.Infrastructure.Services.Correo.Factory
{
    public static class CorreoEstrategiaFactory
    {
        public static Result<ICorreoEstrategia> FactoryCorreo(string tipo)
        {
            ICorreoEstrategia estrategia;
            switch (tipo.ToLower())
            {
                case "confirmacion":
                    estrategia = new CorreoConfirmacionEstrategia();
                    break;
                case "cancelacion":
                    estrategia = new CorreoCancelacionEstrategia();
                    break;
                default:
                    return Result<ICorreoEstrategia>.Failure("Tipo de correo no valido");
            }

            return Result<ICorreoEstrategia>.Success(estrategia);
        }
    }
    
}
