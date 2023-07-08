using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mzeey.Entities;
using Mzeey.DbContextLib;
using System.Collections.Concurrent;
using Mzeey.SharedLib.Utilities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Mzeey.Repositories
{
    public class OrganisationSpaceRepository : IOrganisationSpaceRepository
    {
        private readonly TaskSchedulerContext _db;
        private static ConcurrentDictionary<string, OrganisationSpace> _organisationSpaceCache;

        public OrganisationSpaceRepository(TaskSchedulerContext db)
        {
            _db = db;

            if (_organisationSpaceCache is null)
            {
                _organisationSpaceCache = new ConcurrentDictionary<string, OrganisationSpace>(
                    _db.OrganisationSpaces.ToDictionary(os => os.Id)
                );
            }

        }

        public async Task<OrganisationSpace> CreateAsync(OrganisationSpace space)
        {
            EntityEntry<OrganisationSpace> added = await _db.OrganisationSpaces.AddAsync(space);
            int affected = await _db.SaveChangesAsync();

            return (affected == 1) ? _organisationSpaceCache.AddOrUpdate(space.Id, space, updateCache) : null;
        }


        public async Task<bool> DeleteAsync(string id)
        {
            var space = _organisationSpaceCache.Values.FirstOrDefault(sp => sp.Id.ToUpper() == id.ToUpper());
            if (space == null)
                return false;

            _db.OrganisationSpaces.Remove(space);
            int affected = await _db.SaveChangesAsync();

            return (affected > 1) ? _organisationSpaceCache.TryRemove(id, out space) : false;
        }

        public async Task<OrganisationSpace> UpdateAsync(string id, OrganisationSpace space)
        {
            var normalizedId = id.ToUpper();
            if(normalizedId != space.Id.ToUpper())
            {
                return null;
            }
            var existingUser = _db.OrganisationSpaces.FirstOrDefault(os => os.Id.ToUpper() == normalizedId);
            if (existingUser == null)
                return null;

            _db.OrganisationSpaces.Update(space);
            int affected = await _db.SaveChangesAsync();

            return (affected == 1) ? updateCache(space.Id, space) : null;
        }

        public async Task<IEnumerable<OrganisationSpace>> RetrieveAllAsync()
        {
            return await Task.Run<IEnumerable<OrganisationSpace>>(() => _organisationSpaceCache.Values);
        }

        public async Task<OrganisationSpace> RetrieveAsync(string spaceId)
        {
            return await Task.Run(() =>
            {
                spaceId = spaceId.ToUpper();
                OrganisationSpace space = _organisationSpaceCache.Values.FirstOrDefault(sp => sp.Id.ToUpper() == spaceId);
                return space;
            });
        }

        public async Task<IEnumerable<OrganisationSpace>> RetrieveAllByCreatorIdAsync(string userId)
        {
            return await Task.Run<IEnumerable<OrganisationSpace>>(() => _organisationSpaceCache.Values.Where(os => os.CreatorId.ToUpper() == userId.ToUpper()));
        }

        private OrganisationSpace updateCache(string id, OrganisationSpace organisationSpace)
        {
            return CacheUtility<string>.UpdateCache(_organisationSpaceCache, id, organisationSpace);
        }
    }
}
