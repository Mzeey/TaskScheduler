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
    public class AuthenticationTokenRepositoryMockHelper : MockHelper<IAuthenticationTokenRepository>
    {
        public override Mock<IAuthenticationTokenRepository> ConfigureRepositoryMock()
        {
            var tokens = GenerateData<AuthenticationToken>(10);

            _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<AuthenticationToken>()))
                .ReturnsAsync((AuthenticationToken token) =>
                {
                    token.Id = tokens.Count + 1;
                    token.Token = GenerateUniqueId(); // Assign a new unique ID
                    tokens.Add(token);
                    return token;
                });

            _repositoryMock.Setup(repo => repo.RetrieveAllAsync()).ReturnsAsync((IEnumerable<AuthenticationToken>)tokens.ToList());

            _repositoryMock.Setup(repo => repo.RetrieveAsync(It.IsAny<int>()))
                .ReturnsAsync((int tokenId) =>
                {
                    AuthenticationToken token = tokens.FirstOrDefault(t => t.Id == tokenId);
                    return token;
                });

            _repositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync((int tokenId) =>
                {
                    var existingToken = tokens.FirstOrDefault(t => t.Id == tokenId);
                    if (existingToken == null)
                    {
                        return false;
                    }

                    tokens.Remove(existingToken);
                    return true;
                });

            return _repositoryMock;
        }

        protected override List<T> GenerateData<T>(int count)
        {
            var tokens = new List<T>();

            for (int i = 0; i < count; i++)
            {
                var token = new AuthenticationToken
                {
                    Id = i + 1,
                    Token = GenerateUniqueId(),
                    UserId = $"user-{i +1 }",
                    IssuedDate = DateTime.Now,
                    ExpirationDate = DateTime.Now.AddDays(1)
                };

                tokens.Add((T)(object) token);
            }

            return tokens;
        }
    }
}
