using Mzeey.DbContextLib;
using Mzeey.Entities;
using Mzeey.SharedLib.Utilities;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.Repositories
{
    public class OrganisationSpaceInvitationRepository : IOrganisationSpaceInvitationRepository
    {
        private readonly TaskSchedulerContext _db;
        private static ConcurrentDictionary<int, OrganisationSpaceInvitation> _invitationCache;

        public OrganisationSpaceInvitationRepository(TaskSchedulerContext db)
        {
            _db = db;
            if(_invitationCache is null)
                _invitationCache = new ConcurrentDictionary<int, OrganisationSpaceInvitation>(
                        _db.OrganisationSpaceInvitations.ToDictionary( osi => osi.Id)
                    );
        }
        public async Task<OrganisationSpaceInvitation> CreateAsync(OrganisationSpaceInvitation invitation)
        {
            if(invitation == null)
            {
                return null;
            }

            await _db.OrganisationSpaceInvitations.AddAsync(invitation);
            int affected = await _db.SaveChangesAsync();
            return (affected == 1) ? _invitationCache.AddOrUpdate(invitation.Id, invitation, updateCache) : null;
        }

        public async Task<bool> DeleteAsync(int invitationId)
        {
            var existingInvitation = _db.OrganisationSpaceInvitations.Find(invitationId);
            if (existingInvitation == null)
            {
                return false;
            }
            _db.OrganisationSpaceInvitations.Remove(existingInvitation);
            int affected = await _db.SaveChangesAsync();
            return (affected > 0)? _invitationCache.TryRemove(invitationId, out existingInvitation): false;
        }

        public Task<IEnumerable<OrganisationSpaceInvitation>> RetrieveAllAsync()
        {
            return Task.Run<IEnumerable<OrganisationSpaceInvitation>>(()=> _invitationCache.Values);
        }

        public Task<IEnumerable<OrganisationSpaceInvitation>> RetrieveAllByInviteeIdAsync(string inviteeId)
        {
            return Task.Run<IEnumerable<OrganisationSpaceInvitation>>(() => _invitationCache.Values.Where(osi => osi.InviteeId.ToUpper() == inviteeId.ToUpper()));
        }

        public Task<IEnumerable<OrganisationSpaceInvitation>> RetrieveAllByInviterIdAsync(string inviterId)
        {
            return Task.Run<IEnumerable<OrganisationSpaceInvitation>>(() => _invitationCache.Values.Where(osi => osi.InviterId.ToUpper() == inviterId.ToUpper()));
        }

        public Task<OrganisationSpaceInvitation> RetrieveAsync(int invitationId)
        {
            return Task.Run<OrganisationSpaceInvitation>(() => _invitationCache.Values.FirstOrDefault(osi => osi.Id == invitationId));
        }

        public Task<OrganisationSpaceInvitation> RetrieveByInvitationTokenAsync(string invitationToken)
        {
            return Task.Run<OrganisationSpaceInvitation>(() => _invitationCache.Values.FirstOrDefault(osi => osi.InvitationToken.ToUpper() == invitationToken.ToUpper()));
        }

        public async Task<OrganisationSpaceInvitation> UpdateAsync(int invitationId, OrganisationSpaceInvitation invitation)
        {
            if(invitationId != invitation.Id)
            {
                return null;
            }
            var existingInvitation = _db.OrganisationSpaceInvitations.Find(invitationId);
            if(existingInvitation is null)
            {
                return null;
            }

            _db.OrganisationSpaceInvitations.Update(invitation);
            int affected = await _db.SaveChangesAsync();
            return (affected == 1) ? _invitationCache.AddOrUpdate(invitationId, invitation, updateCache) : null;
        }

        private OrganisationSpaceInvitation updateCache(int id, OrganisationSpaceInvitation invitation)
        {
            return CacheUtility<int>.UpdateCache(_invitationCache, id, invitation);
        }
    }
}
