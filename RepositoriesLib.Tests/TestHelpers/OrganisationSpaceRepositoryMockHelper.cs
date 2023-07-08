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
    public class OrganisationSpaceRepositoryMockHelper : MockHelper<IOrganisationSpaceRepository>
    {
        public override Mock<IOrganisationSpaceRepository> ConfigureRepositoryMock()
        {
            var spaces = GenerateData<OrganisationSpace>(10);

            _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<OrganisationSpace>()))
                .ReturnsAsync((OrganisationSpace space) =>
                {
                    space.Id = Guid.NewGuid().ToString(); // Assign a new unique ID
                    spaces.Add(space);
                    return space;
                });

            _repositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<OrganisationSpace>()))
                .ReturnsAsync((string id, OrganisationSpace space) =>
                {
                    var existingSpace = spaces.FirstOrDefault(s => s.Id.ToUpper() == id.ToUpper());
                    if (existingSpace == null)
                    {
                        return existingSpace;
                    }

                    existingSpace = space;
                    return existingSpace;
                });

            _repositoryMock.Setup(repo => repo.RetrieveAllAsync())
                .ReturnsAsync(() => spaces);

            _repositoryMock.Setup(repo => repo.RetrieveAsync(It.IsAny<string>()))
                .ReturnsAsync((string spaceId) => spaces.FirstOrDefault(s => s.Id == spaceId));

            _repositoryMock.Setup(repo => repo.RetrieveAllByCreatorIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string userId) =>
                {
                    return spaces.Where(os => os.CreatorId.ToUpper() == userId.ToUpper());
                });

            _repositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<string>()))
                .ReturnsAsync((string spaceId) =>
                {
                    var existingSpace = spaces.FirstOrDefault(s => s.Id == spaceId);
                    if (existingSpace == null)
                    {
                        return false;
                    }

                    spaces.Remove(existingSpace);
                    return true;
                });

            return _repositoryMock;
        }

        protected override List<T> GenerateData<T>(int count)
        {
            var spaces = new List<T>();
            for (int i = 1; i <= count; i++)
            {
                var space = new OrganisationSpace
                {
                    Id = $"space_id - {i}",
                    Title = "Organisation Space " + i,
                    Description = "Description for Organisation Space " + i,
                    CreatorId = $"user-{i}",
                    IsPrivate = new Random().Next(2) == 0
                };

                spaces.Add((T)(object)space);
            }

            return spaces;
        }
    }
}
