using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace SistemaGestionCitas.Application.Validators
{
    public class IDValido : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult("El campo no puede ser nulo.");
            }

            if (value is int intValue)
            {
                if (intValue <= 0)
                {
                    return new ValidationResult(ErrorMessage ?? "El valor debe ser un numero positivo.");
                }
            }
            else if (value is short shortValue)
            {
                if (shortValue <= 0)
                {
                    return new ValidationResult(ErrorMessage ?? "El valor debe ser un numero positivo.");
                }
            }
            else
            {
                return new ValidationResult("El tipo de dato no es valido.");
            }
            return ValidationResult.Success;
        }
    }
}
