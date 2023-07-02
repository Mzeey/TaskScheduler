using FluentNHibernate.Conventions.Inspections;
using Moq;
using Mzeey.Entities;
using Mzeey.Repositories;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace RepositoriesLib.Tests.TestHelpers;
public class MockHelper
{
    private Mock<IUserRepository> userRepositoryMock;
    private Mock<IRoleRepository> roleRepositoryMock;
    private Mock<ITaskItemRepository> taskItemRepositoryMock;
    private Mock<IOrganisationSpaceRepository> organisationSpaceRepositoryMock;
    private Mock<IOrganisationUserSpaceRepository> organisationUserSpaceRepositoryMock;
    private Mock<ITaskItemCommentRepository> taskItemCommentRepositoryMock;
    private Mock<IAuthenticationTokenRepository> authenticationTokenRepositoryMock;

    public MockHelper()
    {
        userRepositoryMock = new Mock<IUserRepository>();
        roleRepositoryMock = new Mock<IRoleRepository>();
        taskItemRepositoryMock = new Mock<ITaskItemRepository>();
        organisationSpaceRepositoryMock = new Mock<IOrganisationSpaceRepository>();
        organisationUserSpaceRepositoryMock = new Mock<IOrganisationUserSpaceRepository>();
        taskItemCommentRepositoryMock = new Mock<ITaskItemCommentRepository>();
        authenticationTokenRepositoryMock = new Mock<IAuthenticationTokenRepository>();
    }

    #region Repository_Configurations
    public Mock<IUserRepository> ConfigureUserRepository()
    {
        var users = GenerateUsers(10);

        userRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync((User user) =>
            {
                user.Id = GenerateUniqueId();
                users.Add(user);
                return user;
            });

        userRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<User>()))
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

        userRepositoryMock.Setup(repo => repo.RetrieveAllAsync())
            .ReturnsAsync(() => users);

        userRepositoryMock.Setup(repo => repo.RetrieveAsync(It.IsAny<string>()))
            .ReturnsAsync((string userId) => users.FirstOrDefault(u => u.Id.ToLower() == userId.ToLower()));

        userRepositoryMock.Setup(repo => repo.RetrieveByUserNameAsync(It.IsAny<string>()))
            .ReturnsAsync((string userName) => users.FirstOrDefault(u => u.Username.ToLower() == userName.ToLower()));

        userRepositoryMock.Setup(repo => repo.RetrieveByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((string email) => users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower()));

        userRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<string>()))
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

        return userRepositoryMock;
    }

    public Mock<IRoleRepository> ConfigureRoleRepository()
    {
        var roles = new List<Role>
            {
                new Role { Id = 1, Title = "Admin"},
                new Role { Id = 1, Title = "Manager"},
                new Role { Id = 1, Title = "User"},
            };

        roleRepositoryMock.Setup(repo => repo.RetrieveAllAsync())
            .ReturnsAsync(() =>
            {
                return roles;
            });

        roleRepositoryMock.Setup(repo => repo.RetrieveByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((int roleId) =>
            {   
                return roles.FirstOrDefault(r => r.Id == roleId);
            });

        roleRepositoryMock.Setup(repo => repo.RetrieveByTitleAsync(It.IsAny<string>()))
            .ReturnsAsync((string title) =>
            {
                return roles.FirstOrDefault(r => r.Title.ToLower() == title.ToLower());
            });

        roleRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Role>()))
            .ReturnsAsync((Role role) =>
            {
                role.Id = roles.Count();
                roles.Add(role);
                return role;
            });

        roleRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Role>()))
            .ReturnsAsync((Role role) =>
            {
                var existingRole = roles.FirstOrDefault(r => r.Id == role.Id);
                if(existingRole == null)
                {
                    return existingRole;
                }
                existingRole.Title = role.Title;
                return role;
            });

        roleRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
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

        return roleRepositoryMock;
    }

    public Mock<ITaskItemRepository> ConfigureTaskItemRepository()
{
    var tasks = GenerateTasks(10);

    taskItemRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<TaskItem>()))
        .ReturnsAsync((TaskItem task) =>
        {
            task.Id = Guid.NewGuid().ToString(); // Assign a new unique ID
            tasks.Add(task);
            return task;
        });

    taskItemRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<TaskItem>()))
        .ReturnsAsync((string id, TaskItem task) =>
        {
            // Perform the update operation on the task with the provided ID
            // and return the updated task object
            return task;
        });

    taskItemRepositoryMock.Setup(repo => repo.RetrieveAllAsync())
        .ReturnsAsync(() => tasks);

    taskItemRepositoryMock.Setup(repo => repo.RetrieveAsync(It.IsAny<string>()))
        .ReturnsAsync((string taskId) =>
        {
            // Find the task with the provided ID and return it
            // If not found, return null
            TaskItem task = tasks.FirstOrDefault(t => t.Id == taskId);
            return task;
        });

    taskItemRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<string>()))
        .ReturnsAsync((string taskId) =>
        {
            // Delete the task with the provided ID
            // Return true if deletion is successful, false otherwise
            var existingTask = tasks.FirstOrDefault(t => t.Id == taskId);
            if (existingTask == null)
            {
                return false;
            }

            tasks.Remove(existingTask);
            return true;
        });

    return taskItemRepositoryMock;
}

    public Mock<IOrganisationSpaceRepository> ConfigureOrganisationSpaceRepository()
    {
        var spaces = GenerateOrganisationSpaces(10);

        organisationSpaceRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<OrganisationSpace>()))
            .ReturnsAsync((OrganisationSpace space) =>
            {
                space.Id = Guid.NewGuid().ToString(); // Assign a new unique ID
                spaces.Add(space);
                return space;
            });

        organisationSpaceRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<OrganisationSpace>()))
            .ReturnsAsync((string id, OrganisationSpace space) =>
            {
                var existingSpace = spaces.FirstOrDefault(s => s.Id == id);
                if (existingSpace == null)
                {
                    return existingSpace;
                }

                existingSpace.Title = space.Title;
                existingSpace.Description = space.Description;

                return existingSpace;
            });

        organisationSpaceRepositoryMock.Setup(repo => repo.RetrieveAllAsync())
            .ReturnsAsync(() => spaces);

        organisationSpaceRepositoryMock.Setup(repo => repo.RetrieveAsync(It.IsAny<string>()))
            .ReturnsAsync((string spaceId) => spaces.FirstOrDefault(s => s.Id == spaceId));

        organisationSpaceRepositoryMock.Setup(repo => repo.RetrieveByUserIdAsync(It.IsAny<string>()))
            .ReturnsAsync((string userId) =>
            {
                var userSpaces = new List<OrganisationSpace>();
                // Retrieve all organization spaces associated with the user with the provided ID
                foreach (var space in spaces)
                {
                    if (space.Users.Any(u => u.Id == userId))
                    {
                        userSpaces.Add(space);
                    }
                }
                return userSpaces;
            });

        organisationSpaceRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<string>()))
            .ReturnsAsync((string spaceId) =>
            {
                var existingSpace = spaces.FirstOrDefault(s => s.Id == spaceId);
                if (existingSpace == null)
                {
                    return false;
                }

                spaces.Remove(existingSpace);
                return true;
            });

        return organisationSpaceRepositoryMock;
    }

    public Mock<IOrganisationUserSpaceRepository> ConfigureOrganisationUserSpaceRepository()
    {
        var userSpaces = GenerateOrganisationUserSpaces(10);

        organisationUserSpaceRepositoryMock.Setup(repo => repo.RetrieveAllAsync())
            .ReturnsAsync(() => userSpaces);

        organisationUserSpaceRepositoryMock.Setup(repo => repo.RetrieveAsync(It.IsAny<string>()))
            .ReturnsAsync((string id) => userSpaces.FirstOrDefault(u => u.Id.ToLower() == id.ToLower()));

        organisationUserSpaceRepositoryMock.Setup(repo => repo.RetrieveByUserIdAsync(It.IsAny<string>()))
            .ReturnsAsync((string userId) => userSpaces.Where(u => u.UserId.ToLower() == userId.ToLower()).ToList());

        organisationUserSpaceRepositoryMock.Setup(repo => repo.RetrieveByOrganisationSpaceIdAsync(It.IsAny<string>()))
            .ReturnsAsync((string spaceId) => userSpaces.Where(u => u.OrganisationSpaceId.ToLower() == spaceId.ToLower()).ToList());

        organisationUserSpaceRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<OrganisationUserSpace>()))
            .ReturnsAsync((OrganisationUserSpace userSpace) =>
            {
                userSpace.Id = Guid.NewGuid().ToString(); // Assign a new unique ID
                userSpaces.Add(userSpace);
                return userSpace;
            });

        organisationUserSpaceRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<OrganisationUserSpace>()))
            .ReturnsAsync((OrganisationUserSpace userSpace) =>
            {
                var existingUserSpace = userSpaces.FirstOrDefault(u => u.Id.ToLower() == userSpace.Id.ToLower());
                if (existingUserSpace == null)
                {
                    return existingUserSpace;
                }

                existingUserSpace.UserId = userSpace.UserId;
                existingUserSpace.OrganisationSpaceId = userSpace.OrganisationSpaceId;

                return existingUserSpace;
            });

        organisationUserSpaceRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<string>()))
            .ReturnsAsync((string id) =>
            {
                var existingUserSpace = userSpaces.FirstOrDefault(u => u.Id.ToLower() == id.ToLower());
                if (existingUserSpace == null)
                {
                    return false;
                }

                userSpaces.Remove(existingUserSpace);
                return true;
            });

        return organisationUserSpaceRepositoryMock;
    }

    public Mock<ITaskItemCommentRepository> ConfigureTaskItemCommentRepository()
    {
        var comments = GenerateTaskItemComments(10);

        taskItemCommentRepositoryMock.Setup(repo => repo.RetreiveByTaskItemIdAsync(It.IsAny<string>()))
            .ReturnsAsync((string taskId) => comments.Where(c => c.TaskItemId == taskId));

        taskItemCommentRepositoryMock.Setup(repo => repo.RetrieveByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((string id) => comments.FirstOrDefault(c => c.Id == id));

        taskItemCommentRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<TaskItemComment>()))
            .ReturnsAsync((TaskItemComment comment) =>
            {
                comments.Add(comment);
                return comment;
            });

        taskItemCommentRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<TaskItemComment>()))
            .ReturnsAsync((TaskItemComment comment) =>
            {
                var existingComment = comments.FirstOrDefault(c => c.Id == comment.Id);
                if (existingComment == null)
                {
                    return false;
                }

                existingComment.Content = comment.Content;
                // Update other properties as needed

                return true;
            });

        taskItemCommentRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<string>()))
            .ReturnsAsync((string id) =>
            {
                var existingComment = comments.FirstOrDefault(c => c.Id == id);
                if (existingComment == null)
                {
                    return false;
                }

                comments.Remove(existingComment);
                return true;
            });

        return taskItemCommentRepositoryMock;
    }

    public Mock<IAuthenticationTokenRepository> ConfigureAuthenticationTokenRepository()
    {
        var tokens = GenerateAuthenticationTokens(10);

        authenticationTokenRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<AuthenticationToken>()))
            .ReturnsAsync((AuthenticationToken token) =>
            {
                token.Id = tokens.Count+1;
                token.Token = GenerateUniqueId(); // Assign a new unique ID
                tokens.Add(token);
                return token;
            });

        authenticationTokenRepositoryMock.Setup(repo => repo.RetrieveAllAsync()).ReturnsAsync((IEnumerable<AuthenticationToken>)tokens.ToList());

        authenticationTokenRepositoryMock.Setup(repo => repo.RetrieveAsync(It.IsAny<int>()))
            .ReturnsAsync((int tokenId) =>
            {
                AuthenticationToken token = tokens.FirstOrDefault(t => t.Id == tokenId);
                return token;
            });

        authenticationTokenRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
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

        return authenticationTokenRepositoryMock;
    }
    #endregion

    #region Helper_Methods



    private List<User> GenerateUsers(int count)
    {
        var users = new List<User>();

        for (int i = 0; i < count; i++)
        {
            var user = new User
            {
                Id = GenerateUniqueId(),
                FirstName = "User",
                LastName = (i + 1).ToString(),
                Username = $"user{i + 1}",
                Password = "password",
                Email = $"user{i + 1}@example.com",
                Salt = "salt"
            };

            users.Add(user);
        }

        return users;
    }

    private List<TaskItem> GenerateTasks(int count)
    {
        var tasks = new List<TaskItem>();
        for (int i = 1; i <= count; i++)
        {
            var taskItem = new TaskItem
            {
                Id = GenerateUniqueId(),
                Title = "Task " + i,
                Description = "Description for Task-" + i,
                Status = GetRandomTaskStatus(),
                DateCreated = DateTime.Now
            };

            tasks.Add(taskItem);
        }

        return tasks;
    }

    private List<OrganisationSpace> GenerateOrganisationSpaces(int count)
    {
        var spaces = new List<OrganisationSpace>();
        for (int i = 1; i <= count; i++)
        {
            var space = new OrganisationSpace
            {
                Id = GenerateUniqueId(),
                Title = "Organisation Space " + i,
                Description = "Description for Organisation Space " + i,
                IsPrivate = new Random().Next(2) == 0
            };

            spaces.Add(space);
        }

        return spaces;
    }

    private List<OrganisationUserSpace> GenerateOrganisationUserSpaces(int count)
    {
        var userSpaces = new List<OrganisationUserSpace>();
        for(int i = 0; i < count; i++)
        {
            var userSpace = new OrganisationUserSpace
            {
                Id = GenerateUniqueId(),

                UserId = GenerateUniqueId(),

                OrganisationSpaceId = GenerateUniqueId(),
            };
            userSpaces.Add(userSpace);
        }
        return userSpaces;
    }

    private List<TaskItemComment> GenerateTaskItemComments(int count)
    {
        var comments = new List<TaskItemComment>();
        for (int i = 1; i <= count; i++)
        {
            var comment = new TaskItemComment
            {
                Id = Guid.NewGuid().ToString(),
                TaskItemId = GenerateUniqueId(),
                Content = "Comment " + i,
                CreatedAt = DateTime.Now,
                UserId = GenerateUniqueId()
            };

            comments.Add(comment);
        }

        return comments;
    }

    private List<AuthenticationToken> GenerateAuthenticationTokens(int count)
    {
        var tokens = new List<AuthenticationToken>();

        for (int i = 0; i < count; i++)
        {
            var token = new AuthenticationToken
            {
                Id = i + 1,
                Token = GenerateUniqueId(),
                UserId = GenerateUniqueId(),
                IssuedDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(1)
            };

            tokens.Add(token);
        }

        return tokens;
    }



    private string GetRandomTaskStatus()
    {
        var statuses = new List<string> { "Pending", "In Progress", "Completed", "Overdue" };
        var random = new Random();
        int index = random.Next(statuses.Count);
        return statuses[index];
    }

    private string GenerateUniqueId()
    {
        // Generate a new unique user ID and return it
        // Implement your own logic for generating unique IDs
        return Guid.NewGuid().ToString();
    }

    #endregion
}

