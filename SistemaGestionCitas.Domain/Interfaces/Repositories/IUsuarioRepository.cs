using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Value_Objects;

namespace SistemaGestionCitas.Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository 
    {
        Task AddAsync(Usuario usuario);
        Task<Usuario?> GetByIdAsync(int id);
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<bool> ExisteCedulaAsync(Cedula cedula);
        Task<bool> ExisteCorreoAsync(Correo correo);
        Task<Usuario?> GetByCorreoAndPasswordAsync(string correo, string password);
    }
}