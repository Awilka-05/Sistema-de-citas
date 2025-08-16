using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGestionCitas.Domain.Interfaces.Services
{
    public interface IService<T,Ttype> where T : class
    {
        Task<T> GetByIdAsync(Ttype id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Ttype id);
        
    }
}
