using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using RepositoriesLib;
using RepositoriesLib.Tests.TestHelpers;
using Mzeey.Entities;
using Mzeey.Repositories;

namespace RepositoriesLib.Tests.Repositories
{
    public class OrganisationUserSpaceRepositoryTests
    {
        private readonly IMockHelper<IOrganisationUserSpaceRepository> mockHelper;
        private readonly IOrganisationUserSpaceRepository organisationUserSpaceRepository;

        public OrganisationUserSpaceRepositoryTests()
        {
            mockHelper = new OrganisationUserSpaceRepositoryMockHelper();
            organisationUserSpaceRepository = mockHelper.ConfigureRepositoryMock().Object;
        }

        [Fact]
        public async Task CreateAsync_Should_Add_OrganisationUserSpace_To_Repository()
        {
            // Arrange
            var newUserSpace = new OrganisationUserSpace
            {
                UserId = "user123",
                OrganisationSpaceId = "space123"
            };

            // Act
            var createdUserSpace = await organisationUserSpaceRepository.CreateAsync(newUserSpace);

            // Assert
            Assert.NotNull(createdUserSpace);
            Assert.NotEmpty(createdUserSpace.Id);
            Assert.Equal(newUserSpace.UserId, createdUserSpace.UserId);
            Assert.Equal(newUserSpace.OrganisationSpaceId, createdUserSpace.OrganisationSpaceId);
        }

        [Fact]
        public async Task RetrieveAllAsync_ReturnsAllOrganisationUserSpaces()
        {
            // Act
            var retrievedUserSpaces = await organisationUserSpaceRepository.RetrieveAllAsync();

            // Assert
            Assert.NotNull(retrievedUserSpaces);
            Assert.Equal(10, retrievedUserSpaces.Count());
        }

        [Fact]
        public async Task RetrieveAsync_ExistingUserSpaceId_ReturnsUserSpace()
        {
            // Arrange
            var existingUserSpaces = await organisationUserSpaceRepository.RetrieveAllAsync();
            var existingUserSpace = existingUserSpaces.FirstOrDefault(); // Pick the first user space

            Assert.NotNull(existingUserSpace); // Ensure there's at least one user space in the repository

            // Act
            var retrievedUserSpace = await organisationUserSpaceRepository.RetrieveAsync(existingUserSpace.Id);

            // Assert
            Assert.NotNull(retrievedUserSpace);
            Assert.Equal(existingUserSpace.Id, retrievedUserSpace.Id);
            Assert.Equal(existingUserSpace.UserId, retrievedUserSpace.UserId);
            Assert.Equal(existingUserSpace.OrganisationSpaceId, retrievedUserSpace.OrganisationSpaceId);
        }

        [Fact]
        public async Task RetrieveByUserIdAsync_ExistingUserId_ReturnsUserSpaces()
        {
            // Arrange
            var existingUserSpaces = await organisationUserSpaceRepository.RetrieveAllAsync();
            var existingUserSpace = existingUserSpaces.FirstOrDefault(); // Pick the first user space

            Assert.NotNull(existingUserSpace); // Ensure there's at least one user space in the repository

            // Act
            var retrievedUserSpaces = await organisationUserSpaceRepository.RetrieveByUserIdAsync(existingUserSpace.UserId);

            // Assert
            Assert.NotNull(retrievedUserSpaces);
            Assert.NotEmpty(retrievedUserSpaces);
            Assert.True(retrievedUserSpaces.All(u => u.UserId == existingUserSpace.UserId));
        }

        [Fact]
        public async Task RetrieveByOrganisationSpaceIdAsync_ExistingOrganisationSpaceId_ReturnsUserSpaces()
        {
            // Arrange
            var existingUserSpaces = await organisationUserSpaceRepository.RetrieveAllAsync();
            var existingUserSpace = existingUserSpaces.FirstOrDefault(); // Pick the first user space

            Assert.NotNull(existingUserSpace); // Ensure there's at least one user space in the repository

            // Act
            var retrievedUserSpaces = await organisationUserSpaceRepository.RetrieveAllByOrganisationSpaceIdAsync(existingUserSpace.OrganisationSpaceId);

            // Assert
            Assert.NotNull(retrievedUserSpaces);
            Assert.NotEmpty(retrievedUserSpaces);
            Assert.True(retrievedUserSpaces.All(u => u.OrganisationSpaceId == existingUserSpace.OrganisationSpaceId));
        }

        [Fact]
        public async Task UpdateAsync_ValidUserSpace_ReturnsUpdatedUserSpace()
        {
            // Arrange
            var existingUserSpaces = await organisationUserSpaceRepository.RetrieveAllAsync();
            var existingUserSpace = existingUserSpaces.FirstOrDefault(); // Pick the first user space

            Assert.NotNull(existingUserSpace); // Ensure there's at least one user space in the repository

            var updatedUserSpace = new OrganisationUserSpace
            {
                Id = existingUserSpace.Id,
                UserId = "updatedUser",
                OrganisationSpaceId = "updatedSpace"
            };

            // Act
            var result = await organisationUserSpaceRepository.UpdateAsync(updatedUserSpace);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingUserSpace.Id, result.Id);
            Assert.Equal(updatedUserSpace.UserId, result.UserId);
            Assert.Equal(updatedUserSpace.OrganisationSpaceId, result.OrganisationSpaceId);
        }

        [Fact]
        public async Task DeleteAsync_ExistingUserSpaceId_UserSpaceDeleted()
        {
            // Arrange
            var existingUserSpaces = await organisationUserSpaceRepository.RetrieveAllAsync();
            var existingUserSpace = existingUserSpaces.FirstOrDefault(); // Pick the first user space

            Assert.NotNull(existingUserSpace); // Ensure there's at least one user space in the repository

            // Assert
            var isDeleted = await organisationUserSpaceRepository.DeleteAsync(existingUserSpace.Id);
            Assert.True(isDeleted);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingUserSpaceId_ReturnsFalse()
        {
            // Arrange
            var nonExistingUserSpaceId = "nonExistingId"; // Assuming non-existing user space ID

            // Act
            var isDeleted = await organisationUserSpaceRepository.DeleteAsync(nonExistingUserSpaceId);

            // Assert
            Assert.False(isDeleted);
        }

    }
}
