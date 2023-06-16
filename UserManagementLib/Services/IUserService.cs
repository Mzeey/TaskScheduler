using Mzeey.Entities;
using Mzeey.SharedLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.UserManagementLib.Services
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(string firstName, string lastName, string username, string password, string email, UserRole userRole);
        Task<User> UpdateUserAsync(string userId, string firstName, string lastName, string email);
        Task<bool> DeleteUserAsync(string userId);
        Task<User> GetUserAsync(string userId);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> ChangeUserRoleAsync(string userId, UserRole newRole);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<string> LoginAsync(string username, string password);
        Task<bool> LogoutAsync(string token);
        string GetLoggedInUserId();
    }
}
