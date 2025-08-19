using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionCitas.Application.Validators
{
    public class ValidarFormatoHora : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is string horaStr)
            {
                TimeSpan temp;
                // Aseguramos que la cadena tenga un formato válido de hora.
                return TimeSpan.TryParse(horaStr, out temp);
            }
            return false;
        }
    }
}
