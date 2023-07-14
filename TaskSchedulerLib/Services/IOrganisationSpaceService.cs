using Mzeey.Entities;
using Mzeey.SharedLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSchedulerLib.Services
{
    public interface IOrganisationSpaceService
    {
        Task<OrganisationSpace> CreateOrganisationSpaceAsync(string userId, string title, string description, OrganisationSpaceType spaceType);
        Task<OrganisationSpace> UpdateOrganisationSpaceAsync(string spaceId, string title, string description);
        Task<bool> DeleteOrganisationSpaceAsync(string spaceId);
        Task<string> SendInvitationAsync(string spaceId, string inviterId, string inviteeId, UserRole role, string encryptionKey);
        Task<bool> AcceptInvitationAsync(string invitationToken, string decryptionKey);
        Task<bool> RejectInvitationAsync(string invitationToken, string decryptionKey);
        Task<bool> DeleteUserFromOrganisationSpaceAsync(string spaceId, string userId);
        Task<IEnumerable<OrganisationSpace>> GetOrganisationSpaces(string userId);
        Task<IEnumerable<OrganisationUserSpace>> GetUsersInOrganisationSpace(string organsationSpaceId);
        Task<OrganisationUserSpace> UpdateUserRoleAsync(string spaceId, string userId, UserRole role);
    }
}
