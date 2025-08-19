
using Microsoft.Extensions.Logging;
using SistemaGestionCitas.Application.DTOs.Requests;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Domain.Result_Pattern;

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
            if (await _usuarioRepository.ExisteCedulaAsync(crearUsuario.Cedula))
            {
                _logger.LogError(
                    "Error al crear un nuevo usuario. (La cedula {userCreate.Cedula} ya existe)",
                    crearUsuario.Cedula);

                return Result<object>.Failure("La cedula ya existe.");
            }

            bool correoExiste = await _usuarioRepository.ExisteCorreoAsync(crearUsuario.Correo);

            if (correoExiste)
            {
                _logger.LogError(
                    "Error al crear un nuevo usuario. (El correo {Correo} ya existe)",
                    crearUsuario.Correo);

                return Result<object>.Failure("El correo ya existe.");
            }

            return Result<object>.Success(new object());
        }
    }
}
