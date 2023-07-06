using System.Threading.Tasks;
using Xunit;
using Mzeey.Entities;
using Mzeey.Repositories;
using RepositoriesLib.Tests.TestHelpers;


namespace RepositoriesLib.Tests.Repositories
{
    public class RoleRepositoryTests
    {
        private readonly IMockHelper<IRoleRepository> _mockHelper;
        private readonly IRoleRepository roleRepository;

        public RoleRepositoryTests()
        {
            _mockHelper = new RoleRepositoryMockHelper();
            roleRepository = _mockHelper.ConfigureRepositoryMock().Object;
        }

        [Fact]
        public async Task CreateAsync_Should_Add_Role_To_Repository()
        {
            // Arrange
            var newRole = new Role
            {
                Title = "New Role"
            };

            // Act
            var createdRole = await roleRepository.CreateAsync(newRole);

            // Assert
            Assert.NotNull(createdRole);
            Assert.NotEqual(0, createdRole.Id);
            Assert.Equal(newRole.Title, createdRole.Title);
        }

        [Fact]
        public async Task RetrieveAllAsync_ReturnsAllRoles()
        {
            // Act
            var retrievedRoles = await roleRepository.RetrieveAllAsync();

            // Assert
            Assert.NotNull(retrievedRoles);
            Assert.Equal(3, retrievedRoles.Count());
        }

        [Fact]
        public async Task RetrieveByIdAsync_ExistingRoleId_ReturnsRole()
        {
            // Arrange
            var existingRoles = await roleRepository.RetrieveAllAsync();
            var existingRole = existingRoles.FirstOrDefault(); // Pick the first role

            Assert.NotNull(existingRole); // Ensure there's at least one role in the repository

            // Act
            var retrievedRole = await roleRepository.RetrieveByIdAsync(existingRole.Id);

            // Assert
            Assert.NotNull(retrievedRole);
            Assert.Equal(existingRole.Id, retrievedRole.Id);
            Assert.Equal(existingRole.Title, retrievedRole.Title);
        }

        [Fact]
        public async Task RetrieveByTitleAsync_ExistingRoleTitle_ReturnsRole()
        {
            // Arrange
            var existingRoles = await roleRepository.RetrieveAllAsync();
            var existingRole = existingRoles.FirstOrDefault(); // Pick the first role

            Assert.NotNull(existingRole); // Ensure there's at least one role in the repository

            // Act
            var retrievedRole = await roleRepository.RetrieveByTitleAsync(existingRole.Title);

            // Assert
            Assert.NotNull(retrievedRole);
            Assert.Equal(existingRole.Id, retrievedRole.Id);
            Assert.Equal(existingRole.Title, retrievedRole.Title);
        }

        [Fact]
        public async Task RetrieveByTitleAsync_NonExistingRoleTitle_ReturnsNull()
        {
            // Arrange
            var nonExistingRoleTitle = "NonExistingRole";

            // Act
            var retrievedRole = await roleRepository.RetrieveByTitleAsync(nonExistingRoleTitle);

            // Assert
            Assert.Null(retrievedRole);
        }

        [Fact]
        public async Task UpdateAsync_ValidRole_ReturnsUpdatedRole()
        {
            // Arrange
            var existingRoles = await roleRepository.RetrieveAllAsync();
            var existingRole = existingRoles.FirstOrDefault(); // Pick the first role

            Assert.NotNull(existingRole); // Ensure there's at least one role in the repository

            var updatedRole = new Role
            {
                Id = existingRole.Id,
                Title = "Updated Role"
            };

            // Act
            var result = await roleRepository.UpdateAsync(updatedRole);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedRole.Id, result.Id);
            Assert.Equal(updatedRole.Title, result.Title);
        }

        [Fact]
        public async Task DeleteAsync_ExistingRoleId_RoleDeleted()
        {
            // Arrange
            var existingRoles = await roleRepository.RetrieveAllAsync();
            var existingRole = existingRoles.FirstOrDefault(); // Pick the first role

            Assert.NotNull(existingRole); // Ensure there's at least one role in the repository

            // Assert
            var isDeleted = await roleRepository.DeleteAsync(existingRole.Id);
            Assert.True(isDeleted);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingRoleId_ReturnFalse()
        {
            // Arrange
            var nonExistingRoleId = 999; // Assuming non-existing role ID

            // Act
            var isDeleted = await roleRepository.DeleteAsync(nonExistingRoleId);

            // Assert
            Assert.False(isDeleted);
        }

    }


}
