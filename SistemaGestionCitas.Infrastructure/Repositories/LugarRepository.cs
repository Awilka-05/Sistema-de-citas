
using Microsoft.EntityFrameworkCore;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Infrastructure.Persistence.BdContext;

namespace SistemaGestionCitas.Infrastructure.Repositories
{
    public class LugarRepository : ILugarRepository
    {
        private readonly SistemaCitasDbContext _context;

        public LugarRepository(SistemaCitasDbContext context)
        {
            _context = context;
        }

        public async Task<Lugar?> GetByIdAsync(short id)
        {
             return await _context.Set<Lugar>().FindAsync(id);

        }
           

        public async Task<IEnumerable<Lugar>> GetAllAsync() {

            return await _context.Set<Lugar>().ToListAsync();
        }
           

        public async Task AddAsync(Lugar entity)
        {
            await _context.Set<Lugar>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Lugar entity)
        {
            var existingEntity = _context.Set<Lugar>()
            .Local
            .FirstOrDefault(e => e.LugarId == entity.LugarId);

            if (existingEntity != null)
            {
 
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
            else
            {
                _context.Set<Lugar>().Update(entity);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteNombreAsync(string nombre)
        {
            return await _context.Set<Lugar>().AnyAsync(l => l.Nombre == nombre);
        }
    }
}
