

using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Domain.Interfaces.Services;
namespace SistemaGestionCitas.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository ?? throw new ArgumentNullException(nameof(usuarioRepository));
        }
        public async Task<Usuario?> GetByIdAsync(int id)
        {
            if (id < 0)
                return null; 
            return await _usuarioRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _usuarioRepository.GetAllAsync();
        }

        public async Task <Usuario?> GetByCorreoAndPasswordAsync(string correo, string password)
        {
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(password))
                return null; 
            return await _usuarioRepository.GetByCorreoAndPasswordAsync(correo, password);
        }
    }
}
