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
            return await _context.Usuarios.ToListAsync();
        }
        public async Task AddAsync(Usuario entity)
        {
             _context.Usuarios.Add(entity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Usuario entity)
        {
             _context.Usuarios.Update(entity);
             await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var usuario = await GetByIdAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Usuario> GetByCorreoAsync(string correo)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo.Value == correo);
        }

        public Task<Usuario?> GetByNCedulaAsync(string cedula)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExisteCedulaAsync(string cedula)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Cedula.Value == cedula);
        }

        public Task<bool> ExisteCorreoAsync(string correo)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario?> GetByCorreoAndPasswordAsync(string correo, string password)
        {
            throw new NotImplementedException();
        }
    }
}
