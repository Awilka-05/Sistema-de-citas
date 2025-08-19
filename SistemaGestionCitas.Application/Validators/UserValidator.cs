
//using Microsoft.Extensions.Logging;
//using SistemaGestionCitas.Domain.Entities;
//using SistemaGestionCitas.Domain.Interfaces.Repositories;
//using SistemaGestionCitas.Domain.Result_Pattern;
//using SistemaGestionCitas.Domain.Value_Objects;

//namespace SistemaGestionCitas.Application.Validators
//{
//    using SistemaGestionCitas.Domain.Entities; // <- Asegúrate de usar el namespace correcto
//    using Microsoft.Extensions.Logging;

//    public class UserValidator
//    {
//        private readonly IUsuarioRepository _usuarioRepository;
//        private readonly ILogger<UserValidator> _logger;

//        public UserValidator(IUsuarioRepository usuarioRepository, ILogger<UserValidator> logger)
//        {
//            _usuarioRepository = usuarioRepository;
//            _logger = logger;
//        }

//        public async Task<Result<object>> ValidarAsync(Usuario usuario)
//        {
//            var cedulaResult = Cedula.Create(usuario.Cedula.Value);
//            if (!cedulaResult.IsSuccess) return Result<object>.Failure(cedulaResult.Error);

//            var nombreResult = Nombre.Create(usuario.Nombre.Value);
//            if (!nombreResult.IsSuccess) return Result<object>.Failure(nombreResult.Error);

//            var correoResult = Correo.Create(usuario.Correo.Value);
//            if (!correoResult.IsSuccess) return Result<object>.Failure(correoResult.Error);

//            var cedulaString = usuario.Cedula.Value;
//            var correoString = usuario.Correo.Value;

//            if (await _usuarioRepository.ExisteCedulaAsync(cedulaResult.Value))
//            {
//                _logger.LogError("La cédula {Cedula} ya existe", usuario.Cedula.Value);
//                return Result<object>.Failure("La cédula ya existe.");
//            }

//            if (await _usuarioRepository.ExisteCorreoAsync(correoResult.Value))
//            {
//                _logger.LogError("El correo {Correo} ya existe", usuario.Correo.Value);
//                return Result<object>.Failure("El correo ya existe.");
//            }

//            return Result<object>.Success(new object());
//        }
//    }
//}
