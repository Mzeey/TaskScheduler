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
    public class OrganisationUserSpaceRepository : IOrganisationUserSpaceRepository
    {
        private readonly TaskSchedulerContext _db;
        public static ConcurrentDictionary<string, OrganisationUserSpace> _userSpaceCache;

        public OrganisationUserSpaceRepository(TaskSchedulerContext db)
        {
            _db = db;
            if(_userSpaceCache is null)
            {
                _userSpaceCache = new ConcurrentDictionary<string, OrganisationUserSpace>(
                    _db.OrganisationUserSpaces.ToDictionary(us => us.Id)
                );
            }
        }
        public async Task<OrganisationUserSpace> CreateAsync(OrganisationUserSpace organisationUserSpace)
        {
            EntityEntry<OrganisationUserSpace> added = await _db.OrganisationUserSpaces.AddAsync(organisationUserSpace);
            int affected = await _db.SaveChangesAsync();

            return (affected == 1) ? _userSpaceCache.AddOrUpdate(organisationUserSpace.Id, organisationUserSpace, updateCache) : null;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var normalizedId = id.ToUpper();
            OrganisationUserSpace existingOrganisationUserSpace = _db.OrganisationUserSpaces.FirstOrDefault(ous => ous.Id.ToUpper() == normalizedId);
            if(existingOrganisationUserSpace is null)
            {
                return false;
            }

            _db.OrganisationUserSpaces.Remove(existingOrganisationUserSpace);
            int affected = await _db.SaveChangesAsync();
            return (affected > 1) ? _userSpaceCache.TryRemove(existingOrganisationUserSpace.Id, out existingOrganisationUserSpace) : false;
        }

        public async Task<IEnumerable<OrganisationUserSpace>> RetrieveAllAsync()
        {
            return await Task.Run(() => _userSpaceCache.Values);
        }

        public async Task<OrganisationUserSpace> RetrieveAsync(string id)
        {
            return await Task.Run( () =>
            {
                id = id.ToUpper();
                return _userSpaceCache.Values.FirstOrDefault(us => us.Id.ToUpper() == id);
            });
        }

        public async Task<IEnumerable<OrganisationUserSpace>> RetrieveAllByOrganisationSpaceIdAsync(string spaceId)
        {
            return await Task.Run(() =>
            {
                spaceId = spaceId.ToUpper();
                return _userSpaceCache.Values.Where(us => us.OrganisationSpaceId.ToUpper() == spaceId);
            });
        }

        public async Task<IEnumerable<OrganisationUserSpace>> RetrieveByUserIdAsync(string userId)
        {
            return await Task.Run(() =>
            {
                userId = userId.ToUpper();
                return _userSpaceCache.Values.Where(us => us.UserId.ToUpper() == userId);
            });
        }

        public async Task<OrganisationUserSpace> UpdateAsync(OrganisationUserSpace organisationUserSpace)
        {
            _db.OrganisationUserSpaces.Update(organisationUserSpace);
            int affected = await _db.SaveChangesAsync();

            return (affected == 1) ? updateCache(organisationUserSpace.Id, organisationUserSpace) : null;
        }

        private OrganisationUserSpace updateCache(string id, OrganisationUserSpace organisationUserSpace)
        {
            return CacheUtility<string>.UpdateCache(_userSpaceCache, id, organisationUserSpace);
        }
    }
}