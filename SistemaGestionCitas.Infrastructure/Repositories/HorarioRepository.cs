using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Infrastructure.Persistence.BdContext;

namespace SistemaGestionCitas.Infrastructure.Repositories
{
    public class HorarioRepository : IHorarioRepository
    {
        private readonly SistemaCitasDbContext _context;

        public HorarioRepository(SistemaCitasDbContext context)
        {
            _context = context;
        }

        public async Task<Horario?> GetByIdAsync(short id)
        {
            return await _context.Horarios.FindAsync(id);
        }

        public async Task<IEnumerable<Horario>> GetAllAsync()
        {
            return await _context.Horarios.ToListAsync();
        }

        public async Task AddAsync(Horario entity)
        {
            await _context.Set<Horario>().AddAsync(entity);
            await _context.SaveChangesAsync();
            
        }

        public async Task UpdateAsync(Horario entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

    }
}
