
using Microsoft.EntityFrameworkCore;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Domain.Value_Objects;
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
            return await _context.Usuarios.ToListAsync();

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
     
        public async Task<bool> ExisteCedulaAsync(Cedula cedula)
        {
            return await _context.Usuarios.AnyAsync(u => u.Cedula.Value == cedula.Value);
        }

        public async Task<bool> ExisteCorreoAsync(Correo correo)
        {
            return await _context.Usuarios.AnyAsync(u => u.Correo.Value == correo.Value);
        }

         public async Task<Usuario?> GetByCorreoAndPasswordAsync(string correo, string password)
        {
                        return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo.Value == correo && u.Contrasena == password);
        }
       
    }
}
