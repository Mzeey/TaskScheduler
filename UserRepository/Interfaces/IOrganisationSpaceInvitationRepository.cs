using Mzeey.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.Repositories
{
    public interface IOrganisationSpaceInvitationRepository
    {
        Task<OrganisationSpaceInvitation> CreateAsync(OrganisationSpaceInvitation invitation);
        Task<OrganisationSpaceInvitation> UpdateAsync(int invitationId, OrganisationSpaceInvitation invitation);
        Task<IEnumerable<OrganisationSpaceInvitation>> RetrieveAllAsync();
        Task<IEnumerable<OrganisationSpaceInvitation>> RetrieveAllByInviteeIdAsync(string inviteeId);
        Task<IEnumerable<OrganisationSpaceInvitation>> RetrieveAllByInviterIdAsync(string inviterId);
        Task<OrganisationSpaceInvitation> RetrieveAsync(int invitationId);
        Task<OrganisationSpaceInvitation> RetrieveByInvitationTokenAsync(string invitationToken);
        Task<bool> DeleteAsync(int invitationId);
    }
}
