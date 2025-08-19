
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Domain.Interfaces.Services;
using SistemaGestionCitas.Infrastructure.Repositories;

namespace SistemaGestionCitas.Application.UseCases
{
    public class RegistrarUsuarioService : IRegistrarUsuario
    {
        private readonly IRepository<Usuario, int> _usuarioRepository;
        // O si tienes interfaz específica:
        // private readonly IUsuarioRepository _usuarioRepository;

        public RegistrarUsuarioService(IRepository<Usuario, int> usuarioRepository)
        {
            _usuarioRepository = usuarioRepository ?? throw new ArgumentNullException(nameof(usuarioRepository));
        }

        public async Task<Usuario> RegistrarUsuarioAsync(Usuario usuario)
        {
            // 1. Validar entrada
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo");

            // 2. Validaciones de negocio
            await ValidarReglasDeNegocioAsync(usuario);

            // 3. Establecer valores por defecto
            usuario.FechaNacimiento = DateTime.UtcNow;
            usuario.Activo = true;

            // 4. Guardar en base de datos
            await _usuarioRepository.AddAsync(usuario);

            return usuario;
        }

        private async Task ValidarReglasDeNegocioAsync(Usuario usuario)
        {
            var errores = new List<string>();

            // Validar que la cédula no exista
            if (await ExisteCedulaAsync(usuario.Cedula.Value))
                errores.Add("Ya existe un usuario con esta cédula");

            // Validar que el email no exista
            if (await ExisteEmailAsync(usuario.Correo.Value))
                errores.Add("Ya existe un usuario con este email");

            // Si hay errores, lanzar excepción
            if (errores.Any())
                throw new InvalidOperationException(string.Join("; ", errores));
        }

        private async Task<bool> ExisteCedulaAsync(string cedula)
        {
            // Si tu repositorio tiene métodos específicos
            if (_usuarioRepository is UsuarioRepository usuarioRepo)
            {
                return await usuarioRepo.ExisteCedulaAsync(cedula);
            }

            // Alternativa usando GetAllAsync
            var usuarios = await _usuarioRepository.GetAllAsync();
            return usuarios.Any(u => u.Cedula.Value == cedula);
        }

        private async Task<bool> ExisteEmailAsync(string email)
        {
            if (_usuarioRepository is UsuarioRepository usuarioRepo)
            {
                return await usuarioRepo.ExisteCorreoAsync(email);
            }

            var usuarios = await _usuarioRepository.GetAllAsync();
            return usuarios.Any(u => u.Correo.Value.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
    }
}
