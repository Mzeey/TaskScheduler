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
    public class RoleRepositoryMockHelper : MockHelper<IRoleRepository>
    {
        public override Mock<IRoleRepository> ConfigureRepositoryMock()
        {
            var roles = GenerateData<Role>(0);

            _repositoryMock.Setup(repo => repo.RetrieveAllAsync())
                .ReturnsAsync(() =>
                {
                    return roles;
                });

            _repositoryMock.Setup(repo => repo.RetrieveByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int roleId) =>
                {
                    return roles.FirstOrDefault(r => r.Id == roleId);
                });

            _repositoryMock.Setup(repo => repo.RetrieveByTitleAsync(It.IsAny<string>()))
                .ReturnsAsync((string title) =>
                {
                    return roles.FirstOrDefault(r => r.Title.ToLower() == title.ToLower());
                });

            _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Role>()))
                .ReturnsAsync((Role role) =>
                {
                    role.Id = roles.Count();
                    roles.Add(role);
                    return role;
                });

            _repositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Role>()))
                .ReturnsAsync((Role role) =>
                {
                    var existingRole = roles.FirstOrDefault(r => r.Id == role.Id);
                    if (existingRole == null)
                    {
                        return existingRole;
                    }
                    existingRole.Title = role.Title;
                    return role;
                });

            _repositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync((int roleId) =>
                {
                    var existingRole = roles.FirstOrDefault(r => r.Id == roleId);
                    if (existingRole == null)
                    {
                        return false;
                    }
                    roles.Remove(existingRole);
                    return true;
                });

            return _repositoryMock;
        }

        protected override List<T> GenerateData<T>(int count)
        {
            return new List<T>
            {
                (T)(object) new Role { Id = 1, Title = "Admin"},
                (T)(object) new Role { Id = 2, Title = "Manager"},
                (T)(object) new Role { Id = 3, Title = "User"},
            };
        }
    }
}
