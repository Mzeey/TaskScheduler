using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Mzeey.DbContextLib;
using Mzeey.Entities;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mzeey.SharedLib.Utilities;

namespace Mzeey.UserManagementLib.Repositories
{
    public class AuthenticationTokenRepository : IAuthenticationTokenRepository
    {
        private static ConcurrentDictionary<int, AuthenticationToken> _tokenCache;
        private readonly TaskSchedulerContext _db;

        public AuthenticationTokenRepository(TaskSchedulerContext db)
        {
            _db = db;
            if (_tokenCache is null)
            {
                _tokenCache = new ConcurrentDictionary<int, AuthenticationToken>(
                    _db.AuthenticationTokens.ToDictionary(t => t.TokenId)
                );
            }
        }

        public async Task<AuthenticationToken> CreateAsync(AuthenticationToken token)
        {
            EntityEntry<AuthenticationToken> added = await _db.AuthenticationTokens.AddAsync(token);
            int affected = await _db.SaveChangesAsync();

            return (affected == 1) ? _tokenCache.AddOrUpdate(token.TokenId, token, UpdateCache) : null;
        }

        public async Task<AuthenticationToken> UpdateAsync(AuthenticationToken token)
        {
            _db.AuthenticationTokens.Update(token);
            int affected = await _db.SaveChangesAsync();

            return (affected == 1) ? UpdateCache(token.TokenId, token) : null;
        }

        public async Task<bool> DeleteAsync(int tokenId)
        {
            var token = await _db.AuthenticationTokens.FindAsync(tokenId);
            if (token == null)
                return false;

            _db.AuthenticationTokens.Remove(token);
            int affected = await _db.SaveChangesAsync();

            return (affected == 1) ? _tokenCache.TryRemove(tokenId, out _) : false;
        }

        public Task<AuthenticationToken> RetrieveAsync(int tokenId)
        {
            return Task.Run<AuthenticationToken>(() =>
            {
                AuthenticationToken token;
                _tokenCache.TryGetValue(tokenId, out token);
                return token;
            });
        }

        public Task<AuthenticationToken> RetrieveByTokenAsync(string token)
        {
            return Task.Run<AuthenticationToken>(() =>
            {
                AuthenticationToken authToken = _tokenCache.Values.FirstOrDefault(t => t.Token == token);
                return authToken;
            });
        }

        public Task<IEnumerable<AuthenticationToken>> RetrieveByUserIdAsync(string userId)
        {
            return Task.Run<IEnumerable<AuthenticationToken>>(() =>
            {
                IEnumerable<AuthenticationToken> tokens = _tokenCache.Values.Where(t => t.UserId == userId);
                return tokens;
            });
        }

        private AuthenticationToken UpdateCache(int tokenId, AuthenticationToken token)
        {
            return CacheUtility<int>.UpdateCache(_tokenCache, tokenId, token);
        }
    }
}
