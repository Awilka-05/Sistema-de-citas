

namespace SistemaGestionCitas.Domain.Interfaces.Repositories
{
    public interface IRepository<T, Ttype> where T : class
    {
        Task<T> GetByIdAsync(Ttype id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Ttype id);
    }
}
