using System.Collections.Generic;
using System.Threading.Tasks;
using Mzeey.Entities;

namespace Mzeey.Repositories
{
    public interface IOrganisationUserSpaceRepository
    {
        Task<IEnumerable<OrganisationUserSpace>> RetrieveAllAsync();
        Task<OrganisationUserSpace> RetrieveAsync(string id);
        Task<IEnumerable<OrganisationUserSpace>> RetrieveByUserIdAsync(string userId);
        Task<IEnumerable<OrganisationUserSpace>> RetrieveAllByOrganisationSpaceIdAsync(string spaceId);
        Task<OrganisationUserSpace> CreateAsync(OrganisationUserSpace organisationUserSpace);
        Task<OrganisationUserSpace> UpdateAsync(OrganisationUserSpace organisationUserSpace);
        Task<bool> DeleteAsync(string id);
    }
}
