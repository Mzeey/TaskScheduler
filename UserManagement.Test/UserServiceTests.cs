using Xunit;
using Moq;
using System.Threading.Tasks;
using Mzeey.Entities;
using Mzeey.SharedLib.Enums;
using Mzeey.Repositories;
using Mzeey.UserManagementLib.Services;

namespace UserManagement.Tests
{
    public class UserServiceTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IAuthenticationTokenRepository> _mockTokenRepository;
        private UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockTokenRepository = new Mock<IAuthenticationTokenRepository>();
            _userService = new UserService(_mockUserRepository.Object, _mockTokenRepository.Object);
        }

        [Fact]
        public async Task CreateUserAsync_ValidInput_ReturnsCreatedUser()
        {
            // Arrange
            string firstName = "John";
            string lastName = "Doe";
            string username = "johndoe";
            string password = "password123";
            string email = "john.doe@example.com";
            UserRole role = UserRole.User;

            User createdUser = new User
            {
                Id = "user1",
                FirstName = firstName,
                LastName = lastName,
                Username = username,
                Email = email
            };

            _mockUserRepository.Setup(repo => repo.RetrieveByUserNameAsync(username))
                .ReturnsAsync((User)null);
            _mockUserRepository.Setup(repo => repo.RetrieveByEmailAsync(email))
                .ReturnsAsync((User)null);
            _mockUserRepository.Setup(repo => repo.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync(createdUser);

            // Act
            User result = await _userService.CreateUserAsync(firstName, lastName, username, password, email, role);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(firstName, result.FirstName);
            Assert.Equal(lastName, result.LastName);
            Assert.Equal(username, result.Username);
            Assert.Equal(email, result.Email);
        }

        [Fact]
        public async Task CreateUserAsync_ExistingUsername_ThrowsArgumentException()
        {
            // Arrange
            string username = "johndoe";

            _mockUserRepository.Setup(repo => repo.RetrieveByUserNameAsync(username))
                .ReturnsAsync(new User());

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _userService.CreateUserAsync("John", "Doe", username, "password123", "john.doe@example.com", UserRole.User)
            );
        }

        [Fact]
        public async Task CreateUserAsync_ExistingEmail_ThrowsArgumentException()
        {
            // Arrange
            string email = "john.doe@example.com";

            _mockUserRepository.Setup(repo => repo.RetrieveByUserNameAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);
            _mockUserRepository.Setup(repo => repo.RetrieveByEmailAsync(email))
                .ReturnsAsync(new User());

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _userService.CreateUserAsync("John", "Doe", "johndoe", "password123", email, UserRole.User)
            );
        }

        [Fact]
        public async Task UpdateUserAsync_ExistingUser_ReturnsUpdatedUser()
        {
            // Arrange
            string userId = "user1";
            string firstName = "John";
            string lastName = "Doe";
            string email = "john.doe@example.com";

            User existingUser = new User
            {
                Id = userId,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com"
            };

            _mockUserRepository.Setup(repo => repo.RetrieveAsync(userId))
                .ReturnsAsync(existingUser);
            _mockUserRepository.Setup(repo => repo.UpdateAsync(userId, It.IsAny<User>()))
                .ReturnsAsync(existingUser);

            // Act
            User result = await _userService.UpdateUserAsync(userId, firstName, lastName, email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal(firstName, result.FirstName);
            Assert.Equal(lastName, result.LastName);
            Assert.Equal(email, result.Email);
        }

        [Fact]
        public async Task UpdateUserAsync_NonExistingUser_ReturnsNull()
        {
            // Arrange
            string userId = "user1";
            string firstName = "John";
            string lastName = "Doe";
            string email = "john.doe@example.com";

            _mockUserRepository.Setup(repo => repo.RetrieveAsync(userId))
                .ReturnsAsync((User)null);

            // Act
            User result = await _userService.UpdateUserAsync(userId, firstName, lastName, email);

            // Assert
            Assert.Null(result);
        }

        // Add more test methods for other UserService functionalities...

    }
}
