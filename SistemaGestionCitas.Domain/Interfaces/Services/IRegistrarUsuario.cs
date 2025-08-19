using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Result_Pattern;

namespace SistemaGestionCitas.Domain.Interfaces.Services
{
    public interface IRegistrarUsuario
    {
        Task<Result<Usuario>> RegistrarUsuarioAsync(Usuario usuario);
    }
}