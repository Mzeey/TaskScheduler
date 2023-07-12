using Microsoft.Extensions.Options;
using Mzeey.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementLib.Services
{
    public interface IAuthenticationService
    {
        Task<string> GenerateAuthenticationToken(string userId, string encryptionKey);
        Task<bool> DeleteAuthenticationToken(string userId, string decryptionKey);
        Task<User> ValidateAuthenticationToken(string authenticationToken, string decryptionKey);
    }
}
