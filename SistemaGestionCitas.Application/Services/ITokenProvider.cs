using SistemaGestionCitas.Domain.Entities;

namespace SistemaGestionCitas.Application.Services;

public interface ITokenProvider
{
    public string Create(Usuario usuario);
}