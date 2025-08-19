using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Result_Pattern;

namespace SistemaGestionCitas.Domain.Interfaces.Services
{
    public interface IService<T,Ttype> where T : class
    {
        Task<Result<T>> GetByIdAsync(Ttype id);
        Task<Result<IEnumerable<T>>> GetAllAsync();
        Task<Result<T>> AddAsync(T entity);
        Task<Result<T>> UpdateAsync(T entity);
 
    }
}
