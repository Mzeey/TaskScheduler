using Moq;
using Mzeey.Entities;
using Mzeey.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesLib.Tests.TestHelpers
{
    public class OrganisationUserSpaceRepositoryMockHelper : MockHelper<IOrganisationUserSpaceRepository>
    {
        public override Mock<IOrganisationUserSpaceRepository> ConfigureRepositoryMock()
        {
            var userSpaces = GenerateData<OrganisationUserSpace>(10);

            _repositoryMock.Setup(repo => repo.RetrieveAllAsync())
                .ReturnsAsync(() => userSpaces);

            _repositoryMock.Setup(repo => repo.RetrieveAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) => userSpaces.FirstOrDefault(u => u.Id.ToLower() == id.ToLower()));

            _repositoryMock.Setup(repo => repo.RetrieveByUserIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string userId) => userSpaces.Where(u => u.UserId.ToLower() == userId.ToLower()).ToList());

            _repositoryMock.Setup(repo => repo.RetrieveByOrganisationSpaceIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string spaceId) => userSpaces.Where(u => u.OrganisationSpaceId.ToLower() == spaceId.ToLower()).ToList());

            _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<OrganisationUserSpace>()))
                .ReturnsAsync((OrganisationUserSpace userSpace) =>
                {
                    userSpace.Id = Guid.NewGuid().ToString(); // Assign a new unique ID
                    userSpaces.Add(userSpace);
                    return userSpace;
                });

            _repositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<OrganisationUserSpace>()))
                .ReturnsAsync((OrganisationUserSpace userSpace) =>
                {
                    var existingUserSpace = userSpaces.FirstOrDefault(u => u.Id.ToLower() == userSpace.Id.ToLower());
                    if (existingUserSpace == null)
                    {
                        return existingUserSpace;
                    }

                    existingUserSpace.UserId = userSpace.UserId;
                    existingUserSpace.OrganisationSpaceId = userSpace.OrganisationSpaceId;

                    return existingUserSpace;
                });

            _repositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    var existingUserSpace = userSpaces.FirstOrDefault(u => u.Id.ToLower() == id.ToLower());
                    if (existingUserSpace == null)
                    {
                        return false;
                    }

                    userSpaces.Remove(existingUserSpace);
                    return true;
                });

            return _repositoryMock;
        }

        protected override List<T> GenerateData<T>(int count)
        {
            var userSpaces = new List<T>();
            for (int i = 0; i < count; i++)
            {
                var userSpace = new OrganisationUserSpace
                {
                    Id = GenerateUniqueId(),

                    UserId = GenerateUniqueId(),

                    OrganisationSpaceId = GenerateUniqueId(),
                };
                userSpaces.Add((T)(object) userSpace);
            }
            return userSpaces;
        }
    }
}
