using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaGestionCitas.Domain.Entities;

namespace SistemaGestionCitas.Domain.Interfaces.Services
{
    public interface IUsuarioService
    {
        Task<Usuario> GetByIdAsync(int id);
        Task<IEnumerable<Usuario>> GetAllAsync();
    }
}