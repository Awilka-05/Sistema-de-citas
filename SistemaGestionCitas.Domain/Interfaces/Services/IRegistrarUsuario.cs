using SistemaGestionCitas.Domain.Entities;

namespace SistemaGestionCitas.Domain.Interfaces.Services
{
    public interface IRegistrarUsuario
    {
        Task<Usuario> RegistrarUsuarioAsync(Usuario usuario);
    }
}