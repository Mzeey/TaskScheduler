using Moq;
using Mzeey.Entities;
using Mzeey.Repositories;
using RepositoriesLib.Tests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RepositoriesLib.Tests.Repositories
{
    public class PasswordResetTokenRepositoryTests
    {
        private readonly IMockHelper<IPasswordResetTokenRepository> _mockHelper;
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;

        public PasswordResetTokenRepositoryTests()
        {
            _mockHelper = new PasswordResetTokenRepositoryMockHelper();
            _passwordResetTokenRepository = _mockHelper.ConfigureRepositoryMock().Object;
        }

        [Fact]
        public async Task CreateAsync_Should_Add_PasswordResetToken_To_Repository()
        {
            // Arrange
            var newPasswordResetToken = new PasswordResetToken
            {
                UserId = "user-1",
                Token = "reset-token-11",
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                IsActive = true
            };

            // Act
            var createdPasswordResetToken = await _passwordResetTokenRepository.CreateAsync(newPasswordResetToken);

            // Assert
            Assert.NotNull(createdPasswordResetToken);
            Assert.Equal(newPasswordResetToken.UserId, createdPasswordResetToken.UserId);
            Assert.Equal(newPasswordResetToken.Token, createdPasswordResetToken.Token);
        }

        [Fact]
        public async Task RetrieveAllAsync_ReturnsAllPasswordResetTokens()
        {
            // Act
            var passwordResetTokens = await _passwordResetTokenRepository.RetrieveAllAsync();

            // Assert
            Assert.NotNull(passwordResetTokens);
            Assert.Equal(10, passwordResetTokens.Count());
        }

        [Fact]
        public async Task RetrieveAsync_ExistingPasswordResetTokenId_ReturnsExistingPasswordResetToken()
        {
            // Arrange
            int passwordResetTokenId = 1;

            // Act
            var existingPasswordResetToken = await _passwordResetTokenRepository.RetrieveAsync(passwordResetTokenId);

            // Assert
            Assert.NotNull(existingPasswordResetToken);
            Assert.Equal(passwordResetTokenId, existingPasswordResetToken.Id);
        }

        [Fact]
        public async Task RetrieveByResetToken_ExistingResetToken_ReturnsExistingPasswordResetToken()
        {
            // Arrange
            string resetToken = "reset-token-1";

            // Act
            var existingPasswordResetToken = await _passwordResetTokenRepository.RetrieveByResetToken(resetToken);

            // Assert
            Assert.NotNull(existingPasswordResetToken);
            Assert.Equal(resetToken, existingPasswordResetToken.Token);
        }

        [Fact]
        public async Task UpdateAsync_ExistingPasswordResetTokenId_ReturnsUpdatedPasswordResetToken()
        {
            // Arrange
            int passwordResetTokenId = 1;
            var existingPasswordResetToken = await _passwordResetTokenRepository.RetrieveAsync(passwordResetTokenId);

            // Act
            existingPasswordResetToken.IsActive = false;
            var updatedPasswordResetToken = await _passwordResetTokenRepository.UpdateAsync(passwordResetTokenId, existingPasswordResetToken);

            // Assert
            Assert.NotNull(updatedPasswordResetToken);
            Assert.False(updatedPasswordResetToken.IsActive);
            Assert.Equal(passwordResetTokenId, updatedPasswordResetToken.Id);
        }

        [Fact]
        public async Task DeleteAsync_ExistingPasswordResetTokenId_DeletesPasswordResetToken()
        {
            // Arrange
            int passwordResetTokenId = 1;
            // Act
            var isDeleted = await _passwordResetTokenRepository.DeleteAsync(passwordResetTokenId);

            // Assert
            Assert.True(isDeleted);
        }
    }
}
