using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Mzeey.DbContextLib;
using Mzeey.Entities;
using Mzeey.SharedLib.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mzeey.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static ConcurrentDictionary<string, User> _userCache;
        private readonly TaskSchedulerContext _db;

        public UserRepository(TaskSchedulerContext db)
        {
            _db = db;
            if (_userCache is null)
            {
                _userCache = new ConcurrentDictionary<string, User>(
                    _db.Users.ToDictionary(u => u.Id)
                );
            }
        }

        public async Task<User> CreateAsync(User user)
        {
            await _db.Users.AddAsync(user);
            int affected = await _db.SaveChangesAsync();

            return (affected == 1) ? _userCache.AddOrUpdate(user.Id, user, updateCache) : null;
        }

        public async Task<User> UpdateAsync(string userId, User user)
        {
            if (userId.ToUpper() != user.Id.ToUpper())
                return null;
            var existingUser = _db.Users.FirstOrDefault(u => u.Id.ToUpper() == userId.ToUpper());
            if(existingUser == null)
            {
                return null;
            }
            _db.Users.Update(user);
            int affected = await _db.SaveChangesAsync();

            return (affected == 1) ? updateCache(user.Id, user) : null;
        }

        public async Task<bool> DeleteAsync(string userId)
        {
            var user = _userCache.Values.FirstOrDefault(u => u.Id.ToUpper() == userId.ToUpper());
            if (user == null)
                return false;

            _db.Users.Remove(user);
            int affected = await _db.SaveChangesAsync();

            return (affected > 1) ? _userCache.TryRemove(userId, out user) : false;
        }

        public Task<User> RetrieveAsync(string userId)
        {
            return Task.Run<User>(() =>
            {
                userId = userId.ToUpper();
                User user = _userCache.Values.FirstOrDefault(u => u.Id.ToUpper() == userId.ToUpper());
                return user;
            });
        }

        public Task<User> RetrieveByUserNameAsync(string username)
        {
            return Task.Run<User>(() =>
            {
                User user = _userCache.Values.FirstOrDefault(u => u.Username.ToUpper() == username.ToUpper());
                return user;
            });
        }

        public Task<User> RetrieveByEmailAsync(string email)
        {
            return Task.Run<User>(() =>
            {
                User user = _userCache.Values.FirstOrDefault(u => u.Email.ToUpper() == email.ToUpper());
                return user;
            });
        }

        public async Task<IEnumerable<User>> RetrieveAllAsync()
        {
            return await Task.Run<IEnumerable<User>>(() => _userCache.Values);
        }

        private User updateCache(string id, User user)
        {
            return CacheUtility<string>.UpdateCache(_userCache, id, user);
        }
    }
}
