using System.Collections.Generic;
using System.Threading.Tasks;
using Mzeey.Entities;

namespace Mzeey.Repositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> RetrieveAllAsync();
        Task<Role> RetrieveByIdAsync(int id);
        Task<Role> RetrieveByTitleAsync(string title);
        Task<Role> CreateAsync(Role role);
        Task<Role> UpdateAsync(Role role);
        Task<bool> DeleteAsync(int id);
    }
}
