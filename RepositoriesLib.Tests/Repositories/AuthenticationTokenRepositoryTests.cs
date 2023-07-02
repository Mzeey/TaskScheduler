using Mzeey.Entities;
using Moq;
using Xunit;
using Mzeey.Repositories;
using RepositoriesLib.Tests.TestHelpers;

namespace RepositoriesLib.Tests.Repositories
{
    public class AuthenticationTokenRepositoryTests
    {
        private readonly MockHelper mockHelper;
        private readonly IAuthenticationTokenRepository authenticationTokenRepository;

        public AuthenticationTokenRepositoryTests()
        {
            mockHelper = new MockHelper();
            authenticationTokenRepository = mockHelper.ConfigureAuthenticationTokenRepository().Object;
        }

        [Fact]
        public async Task CreateAsync_Should_Add_AuthenticationToken_To_Repository()
        {
            // Arrange
            var newToken = new AuthenticationToken
            {
                UserId = "User123",
                Token = "abc123",
                ExpirationDate = DateTime.Now.AddDays(1),
                IssuedDate = DateTime.Now
            };

            // Act
            var createdToken = await authenticationTokenRepository.CreateAsync(newToken);

            // Assert
            Assert.NotNull(createdToken);
            Assert.NotEqual(0, createdToken.Id);
            Assert.Equal(newToken.UserId, createdToken.UserId);
            Assert.Equal(newToken.Token, createdToken.Token);
            Assert.Equal(newToken.ExpirationDate, createdToken.ExpirationDate);
            Assert.Equal(newToken.IssuedDate, createdToken.IssuedDate);
        }

        [Fact]
        public async Task RetrieveAsync_ExistingTokenId_ReturnsAuthenticationToken()
        {
            // Arrange
            var existingTokens = await authenticationTokenRepository.RetrieveAllAsync();
            var existingToken = existingTokens.FirstOrDefault(); // Pick the first token

            Assert.NotNull(existingToken); // Ensure there's at least one token in the repository

            // Act
            var retrievedToken = await authenticationTokenRepository.RetrieveAsync(existingToken.Id);

            // Assert
            Assert.NotNull(retrievedToken);
            Assert.Equal(existingToken.Id, retrievedToken.Id);
            Assert.Equal(existingToken.UserId, retrievedToken.UserId);
            Assert.Equal(existingToken.Token, retrievedToken.Token);
            Assert.Equal(existingToken.ExpirationDate, retrievedToken.ExpirationDate);
            Assert.Equal(existingToken.IssuedDate, retrievedToken.IssuedDate);
        }

        [Fact]
        public async Task DeleteAsync_ExistingTokenId_TokenDeleted()
        {
            // Arrange
            var existingTokens = await authenticationTokenRepository.RetrieveAllAsync();
            var existingToken = existingTokens.FirstOrDefault(); // Pick the first token

            Assert.NotNull(existingToken); // Ensure there's at least one token in the repository

            // Act
            var isDeleted = await authenticationTokenRepository.DeleteAsync(existingToken.Id);

            // Assert
            Assert.True(isDeleted);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingTokenId_ReturnFalse()
        {
            // Arrange
            var nonExistingTokenId = 999; // Assuming non-existing token ID

            // Act
            var isDeleted = await authenticationTokenRepository.DeleteAsync(nonExistingTokenId);

            // Assert
            Assert.False(isDeleted);
        }
    }
}
