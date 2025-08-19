
using Microsoft.EntityFrameworkCore;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Infrastructure.Persistence.BdContext;

namespace SistemaGestionCitas.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly SistemaCitasDbContext _context;

        public UsuarioRepository(SistemaCitasDbContext context)
        {
            _context = context;
        }

        //solo es de prueba
        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Set<Usuario>().FindAsync(id);

        }

       
        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public async Task AddAsync(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo.");
            }
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

        }
     
        public async Task<bool> ExisteCedulaAsync(string cedula)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExisteCorreoAsync(string correo)
        {
            throw new NotImplementedException();
        }

         public async Task<Usuario?> GetByCorreoAndPasswordAsync(string correo, string password)
        {
                        return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo.Value == correo && u.Contrasena == password);
        }
       
    }
}
