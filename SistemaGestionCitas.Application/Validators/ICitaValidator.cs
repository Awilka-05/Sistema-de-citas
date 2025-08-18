using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Result_Pattern;

namespace SistemaGestionCitas.Application.Validators
{
    public interface ICitaValidator
    {
        Task<Result<Cita>> ValidarCreacionAsync(Cita entity);
        Task<Result<Cita>> ValidarCancelacionAsync(int citaId, Usuario usuario);
    }
}
