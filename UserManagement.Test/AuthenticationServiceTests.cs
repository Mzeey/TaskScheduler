using Mzeey.Repositories;
using Mzeey.UserManagementLib.Services;
using RepositoriesLib.Tests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly string _enrcryptionKey = "testKey";
        public AuthenticationServiceTests() {
            _authenticationTokenRepository = new AuthenticationTokenRepositoryMockHelper().ConfigureRepositoryMock().Object;
            _userRepository = new UserRepositoryMockHelper().ConfigureRepositoryMock().Object;
            _authenticationService = new AuthenticationService(_authenticationTokenRepository, _userRepository);
        }

        [Fact]
        public async Task GenerateUserAuthenticationToken_ValidUserId_ReturnsEncryptedAuthenticationToken()
        {
            string userId = "user-1";

            string generatedToken = await _authenticationService.GenerateAuthenticationToken(userId, _enrcryptionKey);

            Assert.NotNull(generatedToken);
        }
    }
}
