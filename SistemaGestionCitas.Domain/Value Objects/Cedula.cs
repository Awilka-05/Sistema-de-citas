using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Result_Pattern;

namespace SistemaGestionCitas.Domain.Value_Objects
{
    public record Cedula
    {
        public string Value { get; }

        private Cedula(string value) => Value = value;

        public static Result<Cedula> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result<Cedula>.Failure("La cédula no puede estar vacía.");

            if (value.Length != 11)
                return Result<Cedula>.Failure("La cédula debe tener 11 dígitos.");

            if (!value.All(char.IsDigit))
                return Result<Cedula>.Failure("La cédula debe contener solo números.");

            return Result<Cedula>.Success(new Cedula(value));
        }
    }
}
