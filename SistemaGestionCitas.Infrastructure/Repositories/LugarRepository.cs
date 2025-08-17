
using Microsoft.EntityFrameworkCore;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Interfaces.Repositories;

namespace SistemaGestionCitas.Infrastructure.Repositories
{
    public class LugarRepository : IRepository<Lugar, short>
    {
        private readonly DbContext _context;

        public LugarRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<Lugar?> GetByIdAsync(short id) =>
            await _context.Set<Lugar>().FindAsync(id);

        public async Task<IEnumerable<Lugar>> GetAllAsync() =>
            await _context.Set<Lugar>().ToListAsync();

        public async Task AddAsync(Lugar entity)
        {
            await _context.Set<Lugar>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Lugar entity)
        {
            _context.Set<Lugar>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(short id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Set<Lugar>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExisteNombreAsync(string nombre)
        {
            return await _context.Set<Lugar>().AnyAsync(l => l.Nombre == nombre);
        }
    }
}
