using SistemaGestionCitas.Domain.Entities;

namespace SistemaGestionCitas.Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario> GetByCorreoAsync(string correo);
        Task<bool> ExisteCedulaAsync(string cedula);
    }
}
