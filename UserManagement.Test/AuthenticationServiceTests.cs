using Moq;
using Mzeey.Entities;
using Mzeey.Repositories;
using Mzeey.SharedLib.Exceptions;
using Mzeey.SharedLib.Utilities;
using Mzeey.UserManagementLib.Services;
using RepositoriesLib.Tests.TestHelpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UserManagementLib.Services;
using Xunit;

namespace UserManagement.Test
{
    public class AuthenticationServiceTests
    {
        private readonly IAuthenticationTokenRepository _authenticationTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly string _encryptionKey = "AAECAwQFBgcICQoLDA0ODw==";
        public AuthenticationServiceTests()
        {
            _authenticationTokenRepository = new AuthenticationTokenRepositoryMockHelper().ConfigureRepositoryMock().Object;
            _userRepository = new UserRepositoryMockHelper().ConfigureRepositoryMock().Object;
            _authenticationService = new AuthenticationService(_authenticationTokenRepository, _userRepository);
        }

        #region GenerateUserAuthenticationToken Test Methods

        [Fact]
        public async Task GenerateUserAuthenticationToken_ValidUserId_ReturnsEncryptedAuthenticationToken()
        {
            string userId = "user-1";

            string generatedToken = await _authenticationService.GenerateAuthenticationToken(userId, _encryptionKey);

            Assert.NotNull(generatedToken);
        }

        [Fact]
        public async Task GenerateUserAuthenticationToken_NonExistingUserId_ThrowsUserNotFoundException()
        {
            string userId = "non_existing_userId";

            await Assert.ThrowsAsync<UserNotFoundException>(async () =>
            {
                await _authenticationService.GenerateAuthenticationToken(userId, _encryptionKey);
            });
        }

        public async Task GenerateUserAuthenticationToken_NullUserId_ThrowsArgumentException()
        {
            string userId = null;

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _authenticationService.GenerateAuthenticationToken(userId, _encryptionKey);
            });
        }

        [Fact]
        public async Task GenerateAuthenticationToken_ValidUserId_ThrowsAuthenticationTokenNotCreatedException()
        {
            string userId = "user-1";

            var authenticationTokenRepositoryMock = new Mock<IAuthenticationTokenRepository>();
            authenticationTokenRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<AuthenticationToken>()))
                .ReturnsAsync((AuthenticationToken)null);

            var authenticationService = new AuthenticationService(authenticationTokenRepositoryMock.Object, _userRepository);

            await Assert.ThrowsAsync<AuthenticationTokenNotCreatedException>(async () =>
            {
                await authenticationService.GenerateAuthenticationToken(userId, _encryptionKey);
            });
        }

        [Fact]
        public async Task GenerateAuthenticationToken_HighLoadScenario_TokensAreUniqueAndCorrectlyAssociated()
        {
            List<string> userIds = new List<string> { "user-1", "user-2", "user-3", "user-4", "user-5", "user-6", "user-7" };

            ConcurrentBag<string> generatedTokens = new ConcurrentBag<string>();
            
            await Task.WhenAll( userIds.Select(async userId=>
            {
                try
                {
                    string token = await _authenticationService.GenerateAuthenticationToken(userId, _encryptionKey);

                    generatedTokens.Add(token);
                }
                catch(Exception ex)
                {
                    Debug.WriteLine($"Error generating token for user {userId}: {ex.Message}");
                }
            }));

            Assert.Equal(userIds.Count, generatedTokens.Count);
            Assert.Equal(userIds.Count, generatedTokens.Select(GetUserIdFromToken).Distinct().Count());
        }

        [Fact]
        public async Task GenerateAuthenticationToken_InvalidEncryptionKey_ThrowsArgumentException()
        {
            string userId = "user-1";
            string invalidEncryptionKey = null;

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _authenticationService.GenerateAuthenticationToken(userId, invalidEncryptionKey);
            });
        }

        #endregion
        #region DeleteAuthenticationToken Test Methods

        [Fact]
        public async Task DeleteAuthenticationToken_ValidToken_DeletesToken()
        {
            string userId = "user-1";

            string generatedToken = EncryptionHelper.Encrypt("authentication-token-1", _encryptionKey);

            
            var result = await _authenticationService.DeleteAuthenticationToken(generatedToken, _encryptionKey);
            
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAuthenticationToken_InvalidToken_ThrowsInvalidAuthenticationTokenException()
        {
            
            string authenticationToken = EncryptionHelper.Encrypt("invalidToken", _encryptionKey);

            await Assert.ThrowsAsync<InvalidAuthenticationTokenException>(async () => await _authenticationService.DeleteAuthenticationToken(authenticationToken, _encryptionKey));
        }

        [Fact]
        public async Task DeleteAuthenticationToken_NullToken_ThrowsArgumentException()
        {
            // Arrange
            string authenticationToken = null;

            await Assert.ThrowsAsync<ArgumentException>(async () => await _authenticationService.DeleteAuthenticationToken(authenticationToken, _encryptionKey));
        }

        [Fact]
        public async Task DeleteAuthenticationToken_NullDecryptionKey_ThrowsArgumentException()
        {
            // Arrange
            string authenticationToken = "validToken";

            await Assert.ThrowsAsync<ArgumentException>(async () => await _authenticationService.DeleteAuthenticationToken(authenticationToken, null));
        }
        #endregion

        #region ValidateAuthenticationToken Test Methods
        [Fact]
        public async Task ValidateAuthenticationToken_ValidToken_ReturnsUser()
        {
            string userId = "user-1";
            string generatedToken = EncryptionHelper.Encrypt("authentication-token-1", _encryptionKey);

            User user = await _authenticationService.ValidateAuthenticationToken(generatedToken, _encryptionKey);

            Assert.NotNull(user);
            Assert.Equal(userId, user.Id);
        }

        [Fact]
        public async Task ValidateAuthenticationToken_InvalidToken_ThrowsInvalidAuthenticationTokenException()
        {
            string authenticationToken = EncryptionHelper.Encrypt("invalidToken", _encryptionKey);

            await Assert.ThrowsAsync<InvalidAuthenticationTokenException>(async () =>
            {
                await _authenticationService.ValidateAuthenticationToken(authenticationToken, _encryptionKey);
            });
        }

        [Fact]
        public async Task ValidateAuthenticationToken_NullToken_ThrowsArgumentException()
        {
            string authenticationToken = null;

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _authenticationService.ValidateAuthenticationToken(authenticationToken, _encryptionKey);
            });
        }

        [Fact]
        public async Task ValidateAuthenticationToken_NullDecryptionKey_ThrowsArgumentException()
        {
            string authenticationToken = "validToken";

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _authenticationService.ValidateAuthenticationToken(authenticationToken, null);
            });
        }

        [Fact]
        public async Task ValidateAuthenticationToken_UserNotFound_ThrowsUserNotFoundException()
        {
            string generatedToken = EncryptionHelper.Encrypt("token-1", _encryptionKey);
            

            var authenticationRepository = new Mock<IAuthenticationTokenRepository>();
            authenticationRepository.Setup(repo => repo.RetrieveByTokenAsync(It.IsAny<string>()))
                .ReturnsAsync((string token) =>
                {
                    var authTokens = new List<AuthenticationToken>{
                        new AuthenticationToken { Id = 1, UserId = "non_existing_user_id", Token = "token-1"}
                    };
                    return authTokens.FirstOrDefault(at => at.Token.ToUpper() == token.ToUpper());
                });

            var authenticationService = new AuthenticationService(authenticationRepository.Object, _userRepository);

            await Assert.ThrowsAsync<UserNotFoundException>(async () =>
            {
                await authenticationService.ValidateAuthenticationToken(generatedToken, _encryptionKey);
            });
        }
        #endregion

        private async Task<string> GetUserIdFromToken(string token)
        {
            
            User user = await _authenticationService.ValidateAuthenticationToken(token, _encryptionKey);
            
            return user.Id;
        }

    }
}
