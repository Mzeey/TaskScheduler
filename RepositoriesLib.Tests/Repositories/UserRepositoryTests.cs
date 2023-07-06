using System;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using RepositoriesLib;
using RepositoriesLib.Tests.TestHelpers;
using Mzeey.Entities;
using Mzeey.Repositories;
using RepositoriesLib.Tests.TestHelpers;

namespace RepositoriesLib.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly IMockHelper<IUserRepository> _mockHelper;
        private readonly IUserRepository userRepository;

        public UserRepositoryTests()
        {
            _mockHelper = new UserRepositoryMockHelper();
            userRepository = _mockHelper.ConfigureRepositoryMock().Object;
        }

        [Fact]
        public async Task CreateUserAsync_ValidUser_ReturnsCreatedUserWithId()
        {
            // Arrange
            var newUser = new User
            {
                FirstName = "John",
                LastName = "Doe",
                Username = "johndoe",
                Password = "password",
                Email = "john.doe@example.com",
                Salt = "salt"
            };

            // Act
            var createdUser = await userRepository.CreateAsync(newUser);

            // Assert
            Assert.NotNull(createdUser);
            Assert.NotEmpty(createdUser.Id);
            Assert.Equal(newUser.FirstName, createdUser.FirstName);
            Assert.Equal(newUser.LastName, createdUser.LastName);
            Assert.Equal(newUser.Username, createdUser.Username);
            Assert.Equal(newUser.Password, createdUser.Password);
            Assert.Equal(newUser.Email, createdUser.Email);
            Assert.Equal(newUser.Salt, createdUser.Salt);
        }

        [Fact]
        public async Task RetrieveUserAsync_ExistingUserId_ReturnsUser()
        {
            // Arrange
            var existingUsers = await userRepository.RetrieveAllAsync();
            var existingUser = existingUsers.FirstOrDefault(); // Pick the first user

            Assert.NotNull(existingUser); // Ensure there's at least one user in the repository

            // Act
            var retrievedUser = await userRepository.RetrieveAsync(existingUser.Id);

            // Assert
            Assert.NotNull(retrievedUser);
            Assert.Equal(existingUser.Id, retrievedUser.Id);
            Assert.Equal(existingUser.FirstName, retrievedUser.FirstName);
            Assert.Equal(existingUser.LastName, retrievedUser.LastName);
            Assert.Equal(existingUser.Username, retrievedUser.Username);
            Assert.Equal(existingUser.Password, retrievedUser.Password);
            Assert.Equal(existingUser.Email, retrievedUser.Email);
            Assert.Equal(existingUser.Salt, retrievedUser.Salt);
        }

        [Fact]
        public async Task RetrieveAllAsync_ReturnsAllUsers()
        {
            // Arrange
            int existingUsersCount = 10; // Retrieve users from the mock setup

            // Act
            var retrievedUsers = await userRepository.RetrieveAllAsync();

            // Assert
            Assert.NotNull(retrievedUsers);
            Assert.Equal(existingUsersCount, retrievedUsers.ToList().Count);
        }

        [Fact]
        public async Task UpdateAsync_ValidUser_ReturnsUpdatedUser()
        {
            // Arrange
            var existingUsers = await userRepository.RetrieveAllAsync();
            var existingUser = existingUsers.FirstOrDefault(); // Pick the first user

            Assert.NotNull(existingUser); // Ensure there's at least one user in the repository

            var updatedUser = new User
            {
                Id = existingUser.Id,
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                Username = "updatedusername",
                Password = "updatedpassword",
                Email = "updated.email@example.com",
                Salt = "updatedsalt"
            };

            // Act
            var result = await userRepository.UpdateAsync(existingUser.Id, updatedUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedUser.Id, result.Id);
            Assert.Equal(updatedUser.FirstName, result.FirstName);
            Assert.Equal(updatedUser.LastName, result.LastName);
            Assert.Equal(updatedUser.Username, result.Username);
            Assert.Equal(updatedUser.Password, result.Password);
            Assert.Equal(updatedUser.Email, result.Email);
            Assert.Equal(updatedUser.Salt, result.Salt);
        }

        [Fact]
        public async Task DeleteAsync_ExistingUserId_UserDeleted()
        {
            // Arrange
            var existingUsers = await userRepository.RetrieveAllAsync();
            var existingUser = existingUsers.FirstOrDefault(); // Pick the first user

            Assert.NotNull(existingUser); // Ensure there's at least one user in the repository

            // Assert
            var isDeleted = await userRepository.DeleteAsync(existingUser.Id);
            Assert.True(isDeleted);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingUserId_ReturnFalse()
        {
            // Arrange
            var nonExistingUserId = "nonexistinguserid";

            // Act
            var isDeleted = await userRepository.DeleteAsync(nonExistingUserId);

            // Assert
            Assert.False(isDeleted);
        }

        [Fact]
        public async Task RetrieveUserAsync_NonExistingUserId_ReturnsNull()
        {
            // Arrange
            var nonExistingUserId = "nonexistinguserid";

            // Act
            var retrievedUser = await userRepository.RetrieveAsync(nonExistingUserId);

            // Assert
            Assert.Null(retrievedUser);
        }

        // Add more test methods to cover other repository methods and edge cases
    }

}
