using Moq;
using Mzeey.Entities;
using Mzeey.Repositories;
using Mzeey.SharedLib.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesLib.Tests.TestHelpers
{
    public class UserRepositoryMockHelper : MockHelper<IUserRepository>
    {
        public override Mock<IUserRepository> ConfigureRepositoryMock()
        {
            var users = GenerateData<User>(10);

            _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync((User user) =>
                {
                    user.Id = GenerateUniqueId();
                    users.Add(user);
                    return user;
                });

            _repositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync((string userId, User user) =>
                {
                    var existingUser = users.FirstOrDefault(u => u.Id == userId);
                    if (existingUser == null)
                    {
                        return existingUser;
                    }

                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.Username = user.Username;
                    existingUser.Password = user.Password;
                    existingUser.Email = user.Email;
                    existingUser.Salt = user.Salt;

                    return existingUser;
                });

            _repositoryMock.Setup(repo => repo.RetrieveAllAsync())
                .ReturnsAsync(() => users);

            _repositoryMock.Setup(repo => repo.RetrieveAsync(It.IsAny<string>()))
                .ReturnsAsync((string userId) => users.FirstOrDefault(u => u.Id.ToLower() == userId.ToLower()));

            _repositoryMock.Setup(repo => repo.RetrieveByUserNameAsync(It.IsAny<string>()))
                .ReturnsAsync((string userName) => users.FirstOrDefault(u => u.Username.ToLower() == userName.ToLower()));

            _repositoryMock.Setup(repo => repo.RetrieveByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((string email) => users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower()));

            _repositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<string>()))
                .ReturnsAsync((string userId) =>
                {
                    var existingUser = users.FirstOrDefault(u => u.Id.ToLower() == userId.ToLower());
                    if (existingUser == null)
                    {
                        return false;
                    }

                    users.Remove(existingUser);
                    return true;
                });

            return _repositoryMock;
        }

        protected override List<T> GenerateData<T>(int count)
        {
            var users = new List<T>();
            var rand = new Random();

            for (int i = 0; i < count; i++)
            {
                var salt = PasswordHasher.GenerateSalt();
                var user = new User
                {
                    Id = $"user-{i + 1}",
                    FirstName = "User",
                    LastName = (i + 1).ToString(),
                    Username = $"user{i + 1}",
                    Password = PasswordHasher.HashPassword("password", salt),
                    Email = $"user{i + 1}@example.com",
                    Salt = salt,
                    IsEmailVerified = rand.Next(2) % 2 == 0,
                    LastLoginDate = null,
                };

                users.Add((T)(object)user);
            }

            return users;
        }
    }
}
