using Mzeey.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.Repositories
{
    public interface IOrganisationSpaceRepository
    {
        Task<OrganisationSpace> CreateAsync(OrganisationSpace space);
        Task<OrganisationSpace> UpdateAsync(string id, OrganisationSpace space);
        Task<IEnumerable<OrganisationSpace>> RetrieveAllAsync();
        Task<OrganisationSpace> RetrieveAsync(string spaceId);
        Task<IEnumerable<OrganisationSpace>> RetrieveAllByCreatorIdAsync(string userId);
        Task<bool> DeleteAsync(string spaceId);
    }
}
