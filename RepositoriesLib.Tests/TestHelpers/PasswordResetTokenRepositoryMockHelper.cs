using Moq;
using Mzeey.Entities;
using Mzeey.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesLib.Tests.TestHelpers
{
    public class PasswordResetTokenRepositoryMockHelper : MockHelper<IPasswordResetTokenRepository>
    {
        public PasswordResetTokenRepositoryMockHelper()
        {
            _repositoryMock = new Mock<IPasswordResetTokenRepository>();
        }
        public override Mock<IPasswordResetTokenRepository> ConfigureRepositoryMock()
        {
            var passwordResetTokens = GenerateData<PasswordResetToken>(10);

            _repositoryMock.Setup(repo => repo.RetrieveAllAsync())
                .ReturnsAsync(()=> passwordResetTokens);

            _repositoryMock.Setup( repo => repo.RetrieveAsync(It.IsAny<int>()))
                .ReturnsAsync((int id)=> passwordResetTokens.FirstOrDefault( prt => prt.Id == id));

            _repositoryMock.Setup(repo => repo.RetrieveByResetToken(It.IsAny<string>()))
                .ReturnsAsync((string token) => passwordResetTokens.FirstOrDefault(prt => prt.Token.ToUpper() == token.ToUpper()));

            _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<PasswordResetToken>()))
                .ReturnsAsync((PasswordResetToken passwordResetToken) =>
                {
                    passwordResetToken.Id = passwordResetTokens.Count + 1;
                    passwordResetTokens.Add(passwordResetToken);
                    return passwordResetToken;
                });

            _repositoryMock.Setup( repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<PasswordResetToken>()))
                .ReturnsAsync((int id, PasswordResetToken passwordResetToken) =>
                {
                    if (id != passwordResetToken.Id)
                        return null;
                    var existingPasswordResetToken = passwordResetTokens.FirstOrDefault(prt => prt.Id == id);
                    if (existingPasswordResetToken == null)
                    {
                        return null;
                    }
                    existingPasswordResetToken = passwordResetToken;
                    return existingPasswordResetToken;
                });

            _repositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    var existingPasswordResetToken = passwordResetTokens.FirstOrDefault(prt => prt.Id == id);
                    if (existingPasswordResetToken == null)
                    {
                        return false;
                    }
                    return passwordResetTokens.Remove(existingPasswordResetToken);
                });

            return _repositoryMock;
        }

        protected override List<T> GenerateData<T>(int count)
        {
            var passwordResetTokens = new List<T>();
            for (int i = 1; i <= count; i++)
            {
                var passwordResetToken = new PasswordResetToken
                {
                    Id = i,
                    ExpiryDate = DateTime.UtcNow,
                    IsActive = true,
                    Token = $"reset-token-{i}",
                    UserId = $"user-{i}"
                };
                passwordResetTokens.Add((T)(object)passwordResetToken);
            }
            return passwordResetTokens;
        }
    }
}
