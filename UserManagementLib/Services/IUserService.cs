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
        Task<User> RegisterUserAsync(string firstName, string lastName, string username, string password, string email);
        Task<string> LoginUserAsync(string username, string password, string encryptionKey);
        Task<bool> LogoutUserAsync(string authenticationToken, string decryptionKey);

        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserAsync(string userId);
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> UpdateUserAsync(string userId, string firstName, string lastName, string email);
        Task<bool> DeleteUserAsync(string userId);

        Task<string> RequestPasswordResetAsync(string email, string encryptionKey);
        Task<bool> ResetPasword(string resetToken, string decryptionKey);
        Task<User> ChangeUserPasswordAsync(string userId, string oldpassword, string newPassword);

    }
}
