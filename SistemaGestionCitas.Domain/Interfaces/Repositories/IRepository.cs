
using System.Linq.Expressions;


namespace SistemaGestionCitas.Domain.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
            Task<T?> GetByIdAsync(int id);
            Task<T?> GetByIdAsync(short id);
            Task<IEnumerable<T>> GetAllAsync();
            Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
            Task<T> CreateAsync(T entity);
            Task<T> UpdateAsync(T entity);
            Task<bool> DeleteAsync(int id);
            Task<bool> DeleteAsync(short id);
            Task<bool> ExistAsync(int id);
            Task<bool> ExistAsync(short id);
    
        }
    }
