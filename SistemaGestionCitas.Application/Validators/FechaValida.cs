using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Result_Pattern;
using SistemaGestionCitas.Domain.Value_Objects;

namespace SistemaGestionCitas.Application.Validators
{
    public class FechaValida : ValidationAttribute
    {
        private readonly string _fechaInicioPropiedad;

        public FechaValida(string fechaInicioPropiedad)
        {
            _fechaInicioPropiedad = fechaInicioPropiedad;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var fechaFin = value as DateTime?;

            // pa obtener la propiedad de fecha de inicio del objeto que se está validando
            var fechaInicioPropiedad = validationContext.ObjectType.GetProperty(_fechaInicioPropiedad);

            if (fechaInicioPropiedad == null)
            {
                return new ValidationResult("La propiedad de fecha de inicio no fue encontrada.");
            }

            var fechaInicio = fechaInicioPropiedad.GetValue(validationContext.ObjectInstance) as DateTime?;

            if (fechaFin.HasValue && fechaInicio.HasValue && fechaFin.Value <= fechaInicio.Value)
            {
                return new ValidationResult(
                    ErrorMessage ?? $"La fecha de finalización debe ser posterior a la fecha de inicio.",
                    new[] { validationContext.MemberName! }
                );
            }

            return ValidationResult.Success;
        }
    }
}
