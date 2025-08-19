
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SistemaGestionCitas.Application.Validators;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Domain.Interfaces.Services;
using SistemaGestionCitas.Domain.Result_Pattern;

public class RegistrarUsuario : IRegistrarUsuario
{
    private readonly IUsuarioRepository _usuarioRepository;
   
    private readonly ILogger<RegistrarUsuario> _logger;

    public RegistrarUsuario(
        IUsuarioRepository usuarioRepository,
   
        ILogger<RegistrarUsuario> logger)
    {
        _usuarioRepository = usuarioRepository;
      
        _logger = logger;
    }

    public async Task<Result<Usuario>> RegistrarUsuarioAsync(Usuario usuario)
    {
  
         await _usuarioRepository.AddAsync(usuario); 

        _logger.LogInformation("Usuario '{Correo}' registrado exitosamente.", usuario.Correo.Value);
        return Result<Usuario>.Success(usuario); 
    }
}