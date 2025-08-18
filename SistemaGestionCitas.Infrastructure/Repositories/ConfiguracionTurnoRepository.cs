using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Infrastructure.Persistence.BdContext;

namespace SistemaGestionCitas.Infrastructure.Repositories
{
    public class ConfiguracionTurnoRepository : IConfiguracionTurnoRepository
    {
        private readonly SistemaCitasDbContext _context;
        public ConfiguracionTurnoRepository(SistemaCitasDbContext context)
        {
            _context = context;
        }
        public async Task<ConfiguracionTurno?> GetByIdAsync(int id) { 

            return await _context.Set<ConfiguracionTurno>().FindAsync(id);
        }
        public async Task<IEnumerable<ConfiguracionTurno>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public async Task AddAsync(ConfiguracionTurno entity)
        {
            throw new NotImplementedException();
        }
        public async Task UpdateAsync(ConfiguracionTurno entity)
        {
            throw new NotImplementedException();
        }
        public async Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ConfiguracionTurno>> GetDisponiblesAsync(DateTime fecha)
        {
            throw new NotImplementedException();
            
        }
        
    }
}
