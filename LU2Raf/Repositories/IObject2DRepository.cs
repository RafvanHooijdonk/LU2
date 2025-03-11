using LU2Raf.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LU2Raf.Repositories
{
    public interface IObject2DRepository
    {
        Task AddAsync(Object2D obj);
        Task<IEnumerable<Object2D>> GetAllAsync(string environmentId);
    }
}
