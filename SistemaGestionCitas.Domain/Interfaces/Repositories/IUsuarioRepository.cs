using SistemaGestionCitas.Domain.Entities;

namespace SistemaGestionCitas.Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario, int>
    {
        Task<Usuario?> GetByCorreoAsync(string correo);
        Task<Usuario?> GetByNCedulaAsync(string cedula);
        Task<bool> ExisteCedulaAsync(string cedula);
        
        Task<bool> ExisteCorreoAsync(string correo);
        Task<Usuario?> GetByCorreoAndPasswordAsync(string correo, string password);
    }
}