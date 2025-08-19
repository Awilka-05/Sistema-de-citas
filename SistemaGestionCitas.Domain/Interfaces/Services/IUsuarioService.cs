
using SistemaGestionCitas.Domain.Entities;

namespace SistemaGestionCitas.Domain.Interfaces.Services
{
    public interface IUsuarioService
    {
        Task<Usuario?> GetByIdAsync(int id);
        Task<IEnumerable<Usuario>> GetAllAsync();

        Task<Usuario?> GetByCorreoAndPasswordAsync(string correo, string password);
    }
}