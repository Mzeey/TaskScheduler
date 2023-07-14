using Xunit;
using Moq;
using System.Threading.Tasks;
using Mzeey.Entities;
using Mzeey.SharedLib.Enums;
using Mzeey.Repositories;
using Mzeey.UserManagementLib.Services;
using UserManagementLib.Services;
using RepositoriesLib.Tests.TestHelpers;
using Mzeey.SharedLib.Exceptions;
using Mzeey.SharedLib.Utilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.Versioning;

namespace UserManagement.Tests
{
    public class UserServiceTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly string _encryptionKey= "AAECAwQFBgcICQoLDA0ODw==";
        private readonly IAuthenticationTokenRepository _authenticationTokenRepository;

        public UserServiceTests()
        {
            _userRepository = new UserRepositoryMockHelper().ConfigureRepositoryMock().Object;
            _authenticationTokenRepository = new AuthenticationTokenRepositoryMockHelper().ConfigureRepositoryMock().Object;

            _authenticationService = new AuthenticationService(_authenticationTokenRepository, _userRepository);
            _userService = new UserService(_userRepository, _authenticationService);
        }

        #region RegisterUserAsync_Tests

        [Fact]
        public async Task RegisterUserAsync_NewUser_ReturnsRegisteredUser()
        {
            string firstName = "John";
            string lastName = "Doe";
            string username = "johndoe";
            string password = "password";
            string email = "john.doe@example.com";

            User registeredUser = await _userService.RegisterUserAsync(firstName, lastName, username, password, email);

            Assert.NotNull(registeredUser);
            Assert.Equal(firstName, registeredUser.FirstName);
            Assert.Equal(lastName, registeredUser.LastName);
            Assert.Equal(username, registeredUser.Username);
            Assert.Equal(email, registeredUser.Email);
        }

        [Fact]
        public async Task RegisterUserAsync_ExistingUsername_ThrowsException()
        {
            string existingUsername = "user1";
            string firstName = "John";
            string lastName = "Doe";
            string username = existingUsername;
            string password = "password";
            string email = "john.doe@example.com";

            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await _userService.RegisterUserAsync(firstName, lastName, username, password, email);
            });
        }

        [Fact]
        public async Task RegisterUserAsync_ExistingEmail_ThrowsException()
        {
            string existingEmail = "user1@example.com";
            string firstName = "John";
            string lastName = "Doe";
            string username = "johndoe";
            string password = "password";
            string email = existingEmail;

            
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await _userService.RegisterUserAsync(firstName, lastName, username, password, email);
            });
        }
        #endregion

        #region LoginUserAsync Tests

        [Fact]
        public async Task LoginUserAsync_ValidCredentials_ReturnsAuthenticationToken()
        {
            string username = "user1";
            string salt = PasswordHasher.GenerateSalt();
            string password = "password";
            string hashedPassword = PasswordHasher.HashPassword("password", salt);
            string encryptionKey = _encryptionKey;
            string expectedToken = EncryptionHelper.Encrypt("validToken", _encryptionKey);

            // Mock the UserRepository to return a user with valid credentials
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.RetrieveByUserNameAsync(username))
                .ReturnsAsync(new User { Username = username, Password = hashedPassword, Salt = salt});

            // Mock the AuthenticationService to return a valid token
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            authenticationServiceMock.Setup(service => service.GenerateAuthenticationToken(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(expectedToken);

            var userService = new UserService(userRepositoryMock.Object, authenticationServiceMock.Object);

            
            string token = await userService.LoginUserAsync(username, password, encryptionKey);

            
            Assert.Equal(expectedToken, token);
            Assert.Equal("validToken", EncryptionHelper.Decrypt(token, _encryptionKey));
        }

        [Fact]
        public async Task LoginUserAsync_InvalidUsername_ThrowsInvalidLoginCredentialsException()
        {
            // Arrange
            string invalidUsername = "invaliduser";
            string password = "password";
            string encryptionKey = _encryptionKey;

            // Mock the UserRepository to return null for the invalid username
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.RetrieveByUserNameAsync(invalidUsername))
                .ReturnsAsync((User)null);

            var userService = new UserService(userRepositoryMock.Object, _authenticationService);

            // Act and Assert
            await Assert.ThrowsAsync<InvalidLoginCredentialsException>(async () =>
            {
                await userService.LoginUserAsync(invalidUsername, password, encryptionKey);
            });
        }

        [Fact]
        public async Task LoginUserAsync_InvalidPassword_ThrowsInvalidLoginCredentialsException()
        {
            string username = "user1";
            string invalidPassword = "invalidpassword";
            string encryptionKey = _encryptionKey;
            string salt = PasswordHasher.GenerateSalt();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.RetrieveByUserNameAsync(username))
                .ReturnsAsync(new User { Username = username, Password = "password", Salt = salt });

            var userService = new UserService(userRepositoryMock.Object, _authenticationService);

            await Assert.ThrowsAsync<InvalidLoginCredentialsException>(async () =>
            {
                await userService.LoginUserAsync(username, invalidPassword, encryptionKey);
            });
        }

        #endregion

        #region LogoutUserAsync_Tests

        [Fact]
        public async  Task LogoutUserAsync_ValidAuthenticationToken_ReturnsTrue()
        {
            string authenticationToken = EncryptionHelper.Encrypt("authentication-token-1", _encryptionKey);
            bool userLoggedout = await _userService.LogoutUserAsync(authenticationToken, _encryptionKey);

            Assert.True(userLoggedout);
        }

        [Fact]
        public async Task LogoutUserAsync_InvalidAuthenticationToken_ReturnsFalse()
        {
            string authenticationToken = EncryptionHelper.Encrypt("authentication-token-invalid", _encryptionKey);

            bool userLoggedout = await _userService.LogoutUserAsync(authenticationToken, _encryptionKey);

            Assert.False(userLoggedout);
        }

        #endregion

        #region GetUserAsync Tests
        [Fact]
        public async Task GetUserAsync_ValidUserId_ReturnsExistingUser()
        {
            string userId = "user-1";
            string username = "user1";

            User existingUser = await _userService.GetUserAsync(userId);

            Assert.NotNull(existingUser);
            Assert.Equal(username, existingUser.Username);
        }

        [Fact]
        public async Task GetUserAsync_InvalidUserId_ThrowsUserNotFoundException()
        {
            string userId = "invalidUserId";

            await Assert.ThrowsAsync<UserNotFoundException>(async () =>
            {
                await _userService.GetUserAsync(userId);
            });
        }

        #endregion

        #region GetUserByUsernameAsync Tests

        [Fact]
        public async Task GetUserByUsernameAsync_ValidUsername_ReturnsExistingUser()
        {
            string username = "user1";

            User user = await _userService.GetUserByUsernameAsync(username);

            Assert.NotNull(user);
            Assert.Equal(username, user.Username);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_InvalidUsername_ThrowsUserNotFoundException()
        {
            string username = "invalidUsername";

            await Assert.ThrowsAsync<UserNotFoundException>( async () =>
            {
                await _userService.GetUserByUsernameAsync(username);
            });
        }

        #endregion

        #region GetAllUsersAsync Tests

        [Fact]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            Assert.NotNull(users);
            Assert.Equal(10, users.Count());
        }

        #endregion

        #region UpdateUserAsync Tests 

        [Fact]
        public async Task UpdateUserAsync_ValidUserDetails_ReturnsUpdatedUser()
        {
            string userId = "user-1";
            string firstName = "John";
            string lastName = "doe";
            string email = "johndoe@example.com";

            User updatedUser = await _userService.UpdateUserAsync(userId, firstName, lastName, email);

            Assert.NotNull(updatedUser);
            Assert.Equal(userId, updatedUser.Id);
            Assert.Equal(firstName, updatedUser.FirstName);
            Assert.Equal(lastName, updatedUser.LastName);
            Assert.Equal(email, updatedUser.Email);
        }

        [Fact]
        public async Task UpdateUserAsync_InvalidUserId_ThrowsUserNotFoundException()
        {
            string userId = "invalidUser";
            string firstName = "John";
            string lastName = "doe";
            string email = "johndoe@example.com";

            await Assert.ThrowsAsync<UserNotFoundException>(async () => await _userService.UpdateUserAsync(userId, firstName, lastName, email));
        }

        [Fact]
        public async Task UpdateUserAsync_ExistingEmail_ThrowsArgumentException()
        {
            string userId = "user-1";
            string firstName = "John";
            string lastName = "doe";
            string email = "user8@example.com";

            await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.UpdateUserAsync(userId, firstName, lastName, email));
        }

        #endregion

        #region DeleteUserAsync Tests

        [Fact]
        public async Task DeleteUserAsync_ValidUserId_ReturnsTrue()
        {
            string userId = "user-1";

            bool userDeleted = await _userService.DeleteUserAsync(userId);

            Assert.True(userDeleted);
        }

        [Fact]
        public async Task DeleteUserAsync_InvalidUserId_ReturnsFalse()
        {
            string userId = "nonexisitngUser";

            bool userDeleted = await _userService.DeleteUserAsync(userId);

            Assert.False(userDeleted);
        }

        #endregion

        #region ChangeUserPasswordAsync Tests

        [Fact]
        public async Task ChangeUserPasswordAsync_ValidPasswordChangeDetails_ReturnsUpdatedUser()
        {
            string userId = "user-1";
            string oldpassword = "password";
            string newPassword = "newPassword";

            User user = await _userService.ChangeUserPasswordAsync(userId, oldpassword, newPassword);

            Assert.NotNull(user);
            Assert.Equal(userId, user.Id);

        }
        [Fact]
        public async Task ChangeUserPasswordAsync_InvalidUserId_ThrowsUserNotFoundException()
        {
            string userId = "invalidUserId";
            string oldpassword = "password";
            string newPassword = "newPassword";

            await Assert.ThrowsAsync<UserNotFoundException>(async () =>
            {
                await _userService.ChangeUserPasswordAsync(userId, oldpassword, newPassword);
            });
        }

        [Fact]
        public async Task ChangeUserPasswordAsync_InvalidOldPassword_ThrowsArgumentException()
        {
            string userId = "user-1";
            string oldpassword = "invalidOldPassword";
            string newPassword = "newPassword";

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _userService.ChangeUserPasswordAsync(userId, oldpassword, newPassword);
            });
        }

        #endregion
    }
}
