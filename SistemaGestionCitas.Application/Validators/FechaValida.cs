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

            var fechaInicioPropiedad = validationContext.ObjectType.GetProperty(_fechaInicioPropiedad);

            if (fechaInicioPropiedad == null)
            {
                return new ValidationResult("La propiedad de fecha de inicio no fue encontrada.");
            }

            var fechaInicio = fechaInicioPropiedad.GetValue(validationContext.ObjectInstance) as DateTime?;

            if (fechaFin.HasValue && fechaInicio.HasValue)
            {
         
                if (fechaFin.Value <= fechaInicio.Value)
                {
                    return new ValidationResult("La fecha de finalización debe ser posterior a la fecha de inicio.", new[] { validationContext.MemberName! });
                }

                if ((fechaFin.Value - fechaInicio.Value).TotalDays > 6)
                {
                    return new ValidationResult("El rango de fechas no puede ser mayor a 6 días.", new[] { validationContext.MemberName! });
                }
            }

            return ValidationResult.Success;
        }
    }
}
