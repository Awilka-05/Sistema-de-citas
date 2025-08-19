
using Microsoft.Extensions.Logging;
using SistemaGestionCitas.Application.DTOs.Requests;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Domain.Result_Pattern;
using SistemaGestionCitas.Domain.Value_Objects;

namespace SistemaGestionCitas.Application.Validators
{
    public class UserValidator
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<UserValidator> _logger;

        public UserValidator(IUsuarioRepository usuarioRepository, ILogger<UserValidator> logger)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        public async Task<Result<object>> ValidarAsync(CrearUsuarioDto crearUsuario)
        {
            // Validar con los vo

            var cedulaResult = Cedula.Create(crearUsuario.Cedula);
            if (!cedulaResult.IsSuccess)
                return Result<object>.Failure(cedulaResult.Error);

            var nombreResult = Nombre.Create(crearUsuario.Nombre);
            if (!nombreResult.IsSuccess)
                return Result<object>.Failure(nombreResult.Error);

            
            var correoResult = Correo.Create(crearUsuario.Correo);
            if (!correoResult.IsSuccess)
                return Result<object>.Failure(correoResult.Error);

            // Validar existencia en bd
            if (await _usuarioRepository.ExisteCedulaAsync(cedulaResult.Value))
            {
                _logger.LogError(
                    "Error al crear un nuevo usuario. (La cédula {Cedula} ya existe)",
                    crearUsuario.Cedula);

                return Result<object>.Failure("La cédula ya existe.");
            }

            if (await _usuarioRepository.ExisteCorreoAsync(correoResult.Value))
            {
                _logger.LogError(
                    "Error al crear un nuevo usuario. (El correo {Correo} ya existe)",
                    crearUsuario.Correo);

                return Result<object>.Failure("El correo ya existe.");
            }

            // Todo OK
            return Result<object>.Success(new object());
        }
    }
}
