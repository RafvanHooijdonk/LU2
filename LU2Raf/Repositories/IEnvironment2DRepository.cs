using LU2Raf.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LU2Raf.Repositories
{
    public interface IEnvironment2DRepository
    {
        Task AddAsync(Environment2D environment);
        Task<IEnumerable<Environment2D>> GetAllAsync();
        Task<IEnumerable<Environment2D>> GetByOwnerUserIdAsync(Guid id); 
        Task DeleteAsync(string environmentName);
    }
}
