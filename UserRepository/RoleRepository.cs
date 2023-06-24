using Microsoft.EntityFrameworkCore.ChangeTracking;
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
    public class RoleRepository : IRoleRepository
    {
        private static ConcurrentDictionary<int, Role> _roleCache;
        private readonly TaskSchedulerContext _db;

        public RoleRepository(TaskSchedulerContext db)
        {
            _db = db;
            if (_roleCache is null)
            {
                _roleCache = new ConcurrentDictionary<int, Role>(
                    _db.Roles.ToDictionary(r => r.Id)
                );
            }
        }

        public async Task<IEnumerable<Role>> RetrieveAllAsync()
        {
            return await Task.Run(() => _roleCache.Values);
        }

        public async Task<Role> RetrieveByIdAsync(int id)
        {
            return await Task.Run(() =>
            {
                return _roleCache.Values.FirstOrDefault(r => r.Id == id);
            });
        }

        public async Task<Role> RetrieveByTitleAsync(string title)
        {
            return await Task.Run(() =>
            {
                title = title.ToLower();
                return _roleCache.Values.FirstOrDefault(r => r.Title.ToLower() == title);
            });
        }

        public async Task<Role> CreateAsync(Role role)
        {
            EntityEntry<Role> added = await _db.Roles.AddAsync(role);
            int affected = await _db.SaveChangesAsync();

            return (affected == 1) ? _roleCache.AddOrUpdate(role.Id, role, updateCache) : null;
        }

        public async Task<Role> UpdateAsync(Role role)
        {
            _db.Roles.Update(role);
            int affected = await _db.SaveChangesAsync();

            return (affected == 1) ? updateCache(role.Id, role) : null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Role role = await RetrieveByIdAsync(id);
            if (role != null)
            {
                _db.Roles.Remove(role);
                int affected = await _db.SaveChangesAsync();
                return affected == 1;
            }
            return false;
        }

        private Role updateCache(int id, Role role)
        {
            return CacheUtility<int>.UpdateCache(_roleCache, id, role);
        }
    }
}
