using Mzeey.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(string userId, User user);
        Task<IEnumerable<User>> RetrieveAllAsync();
        Task<User> RetrieveAsync(string userId);
        Task<User> RetrieveByUserNameAsync(string userName);
        Task<User> RetrieveByEmailAsync(string email);
        Task<bool> DeleteAsync(string userId);
    }
}
