
using Microsoft.EntityFrameworkCore;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Interfaces.Repositories;

namespace SistemaGestionCitas.Infrastructure.Repositories
{
    public class ServicioRepository : IRepository<Servicio, int>
    {
        private readonly DbContext _context;

        public ServicioRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<Servicio?> GetByIdAsync(int id) =>
            await _context.Set<Servicio>().FindAsync(id);

        public async Task<IEnumerable<Servicio>> GetAllAsync() =>
            await _context.Set<Servicio>().ToListAsync();

        public async Task AddAsync(Servicio entity)
        {
            await _context.Set<Servicio>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Servicio entity)
        {
            _context.Set<Servicio>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Set<Servicio>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExisteNombreAsync(string nombre)
        {
            return await _context.Set<Servicio>().AnyAsync(s => s.Nombre == nombre);
        }
    }

}
