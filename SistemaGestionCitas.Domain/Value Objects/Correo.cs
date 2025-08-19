using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Result_Pattern;

namespace SistemaGestionCitas.Domain.Value_Objects
{
    public record Correo
    {
        private static readonly Regex EmailRegex =
            new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Value { get; }

        public Correo(string value) => Value = value;

        public static Result<Correo> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result<Correo>.Failure("El correo no puede estar vacío.");

            if (!EmailRegex.IsMatch(value))
                return Result<Correo>.Failure("El correo no tiene un formato válido.");

            return Result<Correo>.Success(new Correo(value));
        }
    }
}