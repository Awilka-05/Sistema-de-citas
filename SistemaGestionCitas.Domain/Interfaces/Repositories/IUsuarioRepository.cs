using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Value_Objects;

namespace SistemaGestionCitas.Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository 
    {
        Task AddAsync(Usuario usuario);
        Task<Usuario?> GetByIdAsync(int id);
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<bool> ExisteCedulaAsync(string cedula);
        Task<bool> ExisteCorreoAsync(string correo);
        Task<Usuario?> GetByCorreoAndPasswordAsync(string correo, string password);
    }
}