﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Mzeey.Entities;

namespace Mzeey.UserManagementLib.Repositories
{
    public interface IAuthenticationTokenRepository
    {
        Task<AuthenticationToken> CreateAsync(AuthenticationToken token);
        Task<AuthenticationToken> UpdateAsync(AuthenticationToken token);
        Task<bool> DeleteAsync(int tokenId);
        Task<AuthenticationToken> RetrieveAsync(int tokenId);
        Task<AuthenticationToken> RetrieveByTokenAsync(string token);
        Task<IEnumerable<AuthenticationToken>> RetrieveByUserIdAsync(string userId);
    }
}
