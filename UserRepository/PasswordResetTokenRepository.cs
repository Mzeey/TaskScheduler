using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Mzeey.DbContextLib;
using Mzeey.Entities;
using Mzeey.SharedLib.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.Repositories
{
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly TaskSchedulerContext _db;
        private readonly ConcurrentDictionary<int, PasswordResetToken> _passwordResetTokenCache;
        public async Task<PasswordResetToken> CreateAsync(PasswordResetToken passwordResetToken)
        {
            await _db.PasswordResetTokens.AddAsync(passwordResetToken);
            int affected = await _db.SaveChangesAsync();
            return (affected == 1) ? _passwordResetTokenCache.AddOrUpdate(passwordResetToken.Id, passwordResetToken, updateCache) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            PasswordResetToken exisitingPasswordResetToken = _db.PasswordResetTokens.Find(id);
            if(exisitingPasswordResetToken is null)
            {
                return false;
            }
            _db.PasswordResetTokens.Remove(exisitingPasswordResetToken);
            int affected = await _db.SaveChangesAsync();
            return (affected > 1) ? _passwordResetTokenCache.TryRemove(id, out exisitingPasswordResetToken) : false;
        }

        public Task<IEnumerable<PasswordResetToken>> RetrieveAllAsync()
        {
            return Task.Run<IEnumerable<PasswordResetToken>>(() => _passwordResetTokenCache.Values);
        }

        public Task<PasswordResetToken> RetrieveAsync(int id)
        {
            return Task.Run(() =>
            {
                PasswordResetToken passwordResetToken;
                _passwordResetTokenCache.TryGetValue(id, out passwordResetToken);
                return passwordResetToken;
            });
        }

        public Task<PasswordResetToken> RetrieveByResetToken(string token)
        {
            return Task.Run(() => {
                return _passwordResetTokenCache.Values.FirstOrDefault(prt => prt.Token.ToUpper() == token.ToUpper());
            });
        }

        public async Task<PasswordResetToken> UpdateAsync(int id, PasswordResetToken passwordResetToken)
        {
            if(id != passwordResetToken.Id)
            {
                return null;
            }

            var exisitingPasswordResetToken = _db.PasswordResetTokens.FirstOrDefault(prt => prt.Id == id);
            if (exisitingPasswordResetToken == null)
                return null;
            _db.PasswordResetTokens.Update(passwordResetToken);
            int affected = await _db.SaveChangesAsync();
            return (affected == 1) ? updateCache(id, passwordResetToken): null;
        }

        private PasswordResetToken updateCache(int id, PasswordResetToken passwordResetToken)
        {
            return CacheUtility<int>.UpdateCache(_passwordResetTokenCache, id, passwordResetToken);
        }
    }
}
