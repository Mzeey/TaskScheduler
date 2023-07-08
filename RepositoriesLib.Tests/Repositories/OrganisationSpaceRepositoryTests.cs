using Moq;
using Mzeey.Entities;
using Mzeey.Repositories;
using RepositoriesLib.Tests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RepositoriesLib.Tests.Repositories
{
    public class OrganisationSpaceRepositoryTests
    {
        private readonly IMockHelper<IOrganisationSpaceRepository> _mockHelper;
        private readonly IOrganisationSpaceRepository _organisationSpaceRepository;

        public OrganisationSpaceRepositoryTests()
        {
            _mockHelper = new OrganisationSpaceRepositoryMockHelper();
            _organisationSpaceRepository = _mockHelper.ConfigureRepositoryMock().Object;
        }

        [Fact]
        public async Task CreateAsync_Should_Add_OrganisationSpace_To_Repository()
        {
            // Arrange
            var newOrganisationSpace = new OrganisationSpace
            {
                Title = "New Organisation Space",
                Description = "Description for the new Organisation Space",
                IsPrivate = false
            };

            // Act
            var createdOrganisationSpace = await _organisationSpaceRepository.CreateAsync(newOrganisationSpace);

            // Assert
            Assert.NotNull(createdOrganisationSpace);
            Assert.NotEmpty(createdOrganisationSpace.Id);
            Assert.Equal(newOrganisationSpace.Title, createdOrganisationSpace.Title);
            Assert.Equal(newOrganisationSpace.Description, createdOrganisationSpace.Description);
            Assert.Equal(newOrganisationSpace.IsPrivate, createdOrganisationSpace.IsPrivate);
        }

        [Fact]
        public async Task UpdateAsync_ExistingOrganisationSpaceId_ReturnsUpdatedOrganisationSpace()
        {
            // Arrange
            var organisationSpaceId = "existing-id";
            var existingOrganisationSpace = await _organisationSpaceRepository.RetrieveAsync(organisationSpaceId);

            Assert.NotNull(existingOrganisationSpace);

            // Update the organisation space
            var updatedOrganisationSpace = new OrganisationSpace
            {
                Id = organisationSpaceId,
                Title = "Updated Organisation Space",
                Description = "Updated description",
                IsPrivate = true
            };

            // Act
            var result = await _organisationSpaceRepository.UpdateAsync(organisationSpaceId, updatedOrganisationSpace);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedOrganisationSpace.Title, result.Title);
            Assert.Equal(updatedOrganisationSpace.Description, result.Description);
            Assert.Equal(updatedOrganisationSpace.IsPrivate, result.IsPrivate);
        }

        [Fact]
        public async Task RetrieveAllAsync_ReturnsAllOrganisationSpaces()
        {
            // Act
            var organisationSpaces = await _organisationSpaceRepository.RetrieveAllAsync();

            // Assert
            Assert.NotNull(organisationSpaces);
            Assert.Equal(10, organisationSpaces.Count());
        }

        [Fact]
        public async Task RetrieveAsync_ExistingOrganisationSpaceId_ReturnsExistingOrganisationSpace()
        {
            // Arrange
            var organisationSpaceId = "space_id - 3";

            // Act
            var existingOrganisationSpace = await _organisationSpaceRepository.RetrieveAsync(organisationSpaceId);

            // Assert
            Assert.NotNull(existingOrganisationSpace);
            Assert.Equal(organisationSpaceId, existingOrganisationSpace.Id);
        }

        [Fact]
        public async Task RetrieveAllByUserIdAsync_ExistingUserId_ReturnsOrganisationSpacesOfUser()
        {
            // Arrange
            var userId = "user-1";

            // Act
            var organisationSpaces = await _organisationSpaceRepository.RetrieveAllByUserIdAsync(userId);

            // Assert
            Assert.NotNull(organisationSpaces);
            Assert.NotEmpty(organisationSpaces);
            Assert.All(organisationSpaces, space => Assert.Contains(userId, space.Users.Select(u => u.Id)));
        }

        [Fact]
        public async Task DeleteAsync_ExistingOrganisationSpaceId_DeletesOrganisationSpace()
        {
            // Arrange
            var organisationSpaceId = "space_id - 3";
            var existingOrganisationSpace = await _organisationSpaceRepository.RetrieveAsync(organisationSpaceId);

            // Act
            var isDeleted = await _organisationSpaceRepository.DeleteAsync(organisationSpaceId);

            // Assert
            Assert.True(isDeleted);
        }
    }
}
