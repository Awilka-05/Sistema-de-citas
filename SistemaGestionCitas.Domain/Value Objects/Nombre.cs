using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Result_Pattern;

namespace SistemaGestionCitas.Domain.Value_Objects
{
    public record Nombre
    {
        public string Value { get; }

        public Nombre(string value) => Value = value;

        public static Result<Nombre> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result<Nombre>.Failure("El nombre no puede estar vacío.");

            if (value.Length < 2)
                return Result<Nombre>.Failure("El nombre es demasiado corto.");

            if (!value.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                return Result<Nombre>.Failure("El nombre solo puede contener letras y espacios.");

            string formatted = char.ToUpper(value[0]) + value.Substring(1).ToLower();

            return Result<Nombre>.Success(new Nombre(formatted));
        }
    }
}