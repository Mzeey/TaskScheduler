using Antlr.Runtime.Tree;
using Moq;
using Mzeey.Entities;
using Mzeey.Repositories;
using Mzeey.SharedLib.Enums;
using Mzeey.SharedLib.Exceptions;
using Mzeey.SharedLib.Extensions;
using Mzeey.SharedLib.Utilities;
using Mzeey.TaskSchedulerLib.Services;
using RepositoriesLib.Tests.TestHelpers;
using System;
using System.Threading.Tasks;
using TaskSchedulerLib.Services;
using Xunit;

namespace TaskScheduler.Tests
{
    public class OrganisationSpaceServiceTests
    {
        private readonly IOrganisationSpaceRepository _organisationSpaceRepository;
        private readonly IOrganisationUserSpaceRepository _organisationUserSpaceRepository;
        private readonly IOrganisationSpaceInvitationRepository _organisationSpaceInvitationRepository;
        private readonly IOrganisationSpaceService _organisationSpaceService;
        private readonly string _encryptionKey = "AAECAwQFBgcICQoLDA0ODw==";

        public OrganisationSpaceServiceTests()
        {
            _organisationSpaceRepository = new OrganisationSpaceRepositoryMockHelper().ConfigureRepositoryMock().Object;
            _organisationUserSpaceRepository = new OrganisationUserSpaceRepositoryMockHelper().ConfigureRepositoryMock().Object;
            _organisationSpaceInvitationRepository = new OrganisationSpaceInvitationMockHelper().ConfigureRepositoryMock().Object;

            _organisationSpaceService = new OrganisationSpaceService(
                _organisationSpaceRepository,
                _organisationUserSpaceRepository,
                _organisationSpaceInvitationRepository);
        }

        #region AcceptInvitationAsync Test Methods

        [Fact]
        public async Task AcceptInvitationAsync_ValidInvitationToken_ReturnsTrue()
        {
           
            string invitationToken = EncryptionHelper.Encrypt("token-1", _encryptionKey);
            string decryptionKey = _encryptionKey;
           
            var result = await _organisationSpaceService.AcceptInvitationAsync(invitationToken, decryptionKey);

            Assert.True(result);
        }

        [Fact]
        public async Task AcceptInvitationAsync_InvalidInvitationToken_ThrowsOrganisationSpaceInvitationNotFoundException()
        {
            string invitationToken = EncryptionHelper.Encrypt("invalidToken", _encryptionKey);
            string decryptionKey = _encryptionKey;
            
            await Assert.ThrowsAsync<OrganisationSpaceInvitationNotFoundException>(async () =>
            {
                await _organisationSpaceService.AcceptInvitationAsync(invitationToken, decryptionKey);
            });
        }

        [Fact]
        public async Task AcceptInvitationAsync_UnableToUpdateInvitation_ThrowsException()
        {
            // Arrange
            string invitationToken = EncryptionHelper.Encrypt("token-1", _encryptionKey);
            string decryptionKey = _encryptionKey;
            string decryptedInvitationToken = "token-1";
            string organisationSpaceId = "space_id - 1";
            string inviteeId = "user-1";
            int roleId = 1;

            var organisationSpaceInvitation = new OrganisationSpaceInvitation
            {
                Id = 1,
                OrganisationSpaceId = organisationSpaceId,
                InviteeId = inviteeId,
                RoleId = roleId,
                InvitationStatus = InvitationStatus.Pending.GetDescription(),
                UpdatedDate = DateTime.UtcNow
            };

            var organisationSpaceInvitationRepositoryMock = new Mock<IOrganisationSpaceInvitationRepository>();
            organisationSpaceInvitationRepositoryMock.Setup(repo => repo.RetrieveByInvitationTokenAsync(decryptedInvitationToken))
                .ReturnsAsync(organisationSpaceInvitation);

            organisationSpaceInvitationRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<OrganisationSpaceInvitation>()))
                .ReturnsAsync((OrganisationSpaceInvitation)null);

            var organisationSpaceService = new OrganisationSpaceService(_organisationSpaceRepository, _organisationUserSpaceRepository, organisationSpaceInvitationRepositoryMock.Object);

            
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await organisationSpaceService.AcceptInvitationAsync(invitationToken, decryptionKey);
            });
        }

        [Fact]
        public async Task AcceptInvitationAsync_UnableToCreateOrganisationUserSpace_ReturnsFalse()
        {
            string invitationToken = EncryptionHelper.Encrypt("token-1", _encryptionKey);
            string decryptionKey = _encryptionKey;
            string decryptedInvitationToken = "token-1";
            string organisationSpaceId = "space_id - 1";
            string inviteeId = "user-1";
            int roleId = 1;

            var organisationSpaceInvitation = new OrganisationSpaceInvitation
            {
                Id = 1,
                OrganisationSpaceId = organisationSpaceId,
                InviteeId = inviteeId,
                RoleId = roleId,
                InvitationStatus = InvitationStatus.Pending.GetDescription(),
                UpdatedDate = DateTime.UtcNow
            };

            var organisationUserSpaceRepositoryMock = new Mock<IOrganisationUserSpaceRepository>();
            organisationUserSpaceRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<OrganisationUserSpace>()))
                .ReturnsAsync(null as OrganisationUserSpace);

            var organisationSpaceService = new OrganisationSpaceService(_organisationSpaceRepository, organisationUserSpaceRepositoryMock.Object, _organisationSpaceInvitationRepository);

            var result = await organisationSpaceService.AcceptInvitationAsync(invitationToken, decryptionKey);

            Assert.False(result);
        }
        #endregion

        #region CreateOrganisationSpaceAsync Test Methods

        [Fact]
        public async Task CreateOrganisationSpaceAsync_ValidArguments_ReturnsOrganisationSpace()
        {
            string userId = "user-1";
            string title = "Space 1";
            string description = "Test space";
            OrganisationSpaceType spaceType = OrganisationSpaceType.Public;

            var result = await _organisationSpaceService.CreateOrganisationSpaceAsync(userId, title, description, spaceType);

            Assert.NotNull(result);
            Assert.Equal(title, result.Title);
            Assert.Equal(userId, result.CreatorId);
            Assert.Equal(description, result.Description);
            Assert.False(result.IsPrivate);
        }

        [Fact]
        public async Task CreateOrganisationSpaceAsync_NullUserId_ThrowsArgumentException()
        {
            // Arrange
            string userId = null;
            string title = "Space 1";
            string description = "Test space";
            OrganisationSpaceType spaceType = OrganisationSpaceType.Public;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _organisationSpaceService.CreateOrganisationSpaceAsync(userId, title, description, spaceType);
            });
        }

        [Fact]
        public async Task CreateOrganisationSpaceAsync_NullTitle_ThrowsArgumentException()
        {
            // Arrange
            string userId = "user-1";
            string title = null;
            string description = "Test space";
            OrganisationSpaceType spaceType = OrganisationSpaceType.Public;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _organisationSpaceService.CreateOrganisationSpaceAsync(userId, title, description, spaceType);
            });
        }

        [Fact]
        public async Task CreateOrganisationSpaceAsync_SpaceNotCreated_ThrowsOrganisationSpaceNotCreatedException()
        {
            // Arrange
            string userId = "user-1";
            string title = "Space 1";
            string description = "Test space";
            OrganisationSpaceType spaceType = OrganisationSpaceType.Public;

            var organisationSpaceRepositoryMock = new Mock<IOrganisationSpaceRepository>();
            organisationSpaceRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<OrganisationSpace>()))
                .ReturnsAsync((OrganisationSpace)null);

            var organisationSpaceService = new OrganisationSpaceService(organisationSpaceRepositoryMock.Object, _organisationUserSpaceRepository, _organisationSpaceInvitationRepository);

            // Act & Assert
            await Assert.ThrowsAsync<OrganisationSpaceNotCreatedException>(async () =>
            {
                await organisationSpaceService.CreateOrganisationSpaceAsync(userId, title, description, spaceType);
            });
        }

        [Fact]
        public async Task CreateOrganisationSpaceAsync_UserSpaceNotCreated_DeletesOrganisationSpaceAndThrowsOrganisationSpaceNotCreatedException()
        {
            // Arrange
            string userId = "user-1";
            string title = "Space 1";
            string description = "Test space";
            OrganisationSpaceType spaceType = OrganisationSpaceType.Public;

            var organisationSpace = new OrganisationSpace
            {
                Id = "space-1",
                Title = title,
                CreatorId = userId,
                Description = description,
                IsPrivate = false
            };

            var organisationSpaceRepositoryMock = new Mock<IOrganisationSpaceRepository>();
            var organisationUserSpaceRepositoryMock = new Mock<IOrganisationUserSpaceRepository>();

            organisationSpaceRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<OrganisationSpace>()))
                .ReturnsAsync(organisationSpace);

            organisationUserSpaceRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<OrganisationUserSpace>()))
                .ReturnsAsync((OrganisationUserSpace)null);

            organisationSpaceRepositoryMock.Setup(repo => repo.DeleteAsync(organisationSpace.Id))
                .ReturnsAsync(true);

            var organisationSpaceService = new OrganisationSpaceService(organisationSpaceRepositoryMock.Object, organisationUserSpaceRepositoryMock.Object, _organisationSpaceInvitationRepository);

            // Act & Assert
            await Assert.ThrowsAsync<OrganisationSpaceNotCreatedException>(async () =>
            {
                await organisationSpaceService.CreateOrganisationSpaceAsync(userId, title, description, spaceType);
            });
        }

        #endregion

        #region DeleteOrganisationSpaceAsync Test Methods

        [Fact]
        public async Task DeleteOrganisationSpaceAsync_OrganisationSpaceExists_DeletesSpaceAndReturnsTrue()
        {
            // Arrange
            string spaceId = "space-1";

            var organisationSpace = new OrganisationSpace
            {
                Id = spaceId,
            };

            var organisationSpaceRepositoryMock = new Mock<IOrganisationSpaceRepository>();
            organisationSpaceRepositoryMock.Setup(repo => repo.RetrieveAsync(spaceId))
                .ReturnsAsync(organisationSpace);
            organisationSpaceRepositoryMock.Setup(repo => repo.DeleteAsync(spaceId))
                .ReturnsAsync(true);

            var organisationUserSpaceRepositoryMock = new Mock<IOrganisationUserSpaceRepository>();
            organisationUserSpaceRepositoryMock.Setup(repo => repo.RetrieveAllByOrganisationSpaceIdAsync(spaceId))
                .ReturnsAsync(new List<OrganisationUserSpace>());

            var organisationSpaceService = new OrganisationSpaceService(organisationSpaceRepositoryMock.Object, organisationUserSpaceRepositoryMock.Object, _organisationSpaceInvitationRepository);

            var result = await organisationSpaceService.DeleteOrganisationSpaceAsync(spaceId);

            
            Assert.True(result);
            organisationSpaceRepositoryMock.Verify(repo => repo.DeleteAsync(spaceId), Times.Once);
            organisationUserSpaceRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<string>()), Times.Never);
            organisationUserSpaceRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<OrganisationUserSpace>()), Times.Never);
        }

        [Fact]
        public async Task DeleteOrganisationSpaceAsync_OrganisationSpaceNotFound_ThrowsOrganisationSpaceNotFoundException()
        {
            string spaceId = "space-1";

            var organisationSpaceRepositoryMock = new Mock<IOrganisationSpaceRepository>();
            organisationSpaceRepositoryMock.Setup(repo => repo.RetrieveAsync(spaceId))
                .ReturnsAsync((OrganisationSpace)null);

            var organisationSpaceService = new OrganisationSpaceService(organisationSpaceRepositoryMock.Object, _organisationUserSpaceRepository, _organisationSpaceInvitationRepository);

            await Assert.ThrowsAsync<OrganisationSpaceNotFoundException>(async () =>
            {
                await organisationSpaceService.DeleteOrganisationSpaceAsync(spaceId);
            });
        }

        [Fact]
        public async Task DeleteOrganisationSpaceAsync_OrganisationSpaceDeletedFalse_RestoresSpaceMembersAndReturnsFalse()
        {
            string spaceId = "space-1";

            var organisationSpace = new OrganisationSpace
            {
                Id = spaceId,
            };

            var organisationSpaceRepositoryMock = new Mock<IOrganisationSpaceRepository>();
            organisationSpaceRepositoryMock.Setup(repo => repo.RetrieveAsync(spaceId))
                .ReturnsAsync(organisationSpace);
            organisationSpaceRepositoryMock.Setup(repo => repo.DeleteAsync(spaceId))
                .ReturnsAsync(false);

            var organisationUserSpaceRepositoryMock = new Mock<IOrganisationUserSpaceRepository>();
            var organisationSpaceMembers = new List<OrganisationUserSpace>
            {
                new OrganisationUserSpace { Id = "user-space-1"},
                new OrganisationUserSpace { Id = "user-space-2"},
            };
            organisationUserSpaceRepositoryMock.Setup(repo => repo.RetrieveAllByOrganisationSpaceIdAsync(spaceId))
                .ReturnsAsync(organisationSpaceMembers);

            var organisationSpaceService = new OrganisationSpaceService(organisationSpaceRepositoryMock.Object, organisationUserSpaceRepositoryMock.Object, _organisationSpaceInvitationRepository);

            var result = await organisationSpaceService.DeleteOrganisationSpaceAsync(spaceId);

            Assert.False(result);
            organisationSpaceRepositoryMock.Verify(repo => repo.DeleteAsync(spaceId), Times.Once);
            organisationUserSpaceRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<string>()), Times.Exactly(organisationSpaceMembers.Count));
            organisationUserSpaceRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<OrganisationUserSpace>()), Times.Exactly(organisationSpaceMembers.Count));
        }

        #endregion

        #region DeleteUserFromOrganisationSpaceAsync Test Methods

        [Fact]
        public async Task DeleteUserFromOrganisationSpaceAsync_ValidArguments_DeletesUserFromSpaceAndReturnsTrue()
        {
            // Arrange
            string spaceId = "space-1";
            string userId = "user-1";

            var organisationSpace = new OrganisationSpace
            {
                Id = spaceId,
                Title = "Space 1"
            };

            var organisationUserSpaces = new List<OrganisationUserSpace>
            {
                new OrganisationUserSpace { Id = "user-space-1", OrganisationSpaceId = spaceId, UserId = userId },
                new OrganisationUserSpace { Id = "user-space-2", OrganisationSpaceId = spaceId, UserId = "user-2" }
            };

            var organisationSpaceRepositoryMock = new Mock<IOrganisationSpaceRepository>();
            organisationSpaceRepositoryMock.Setup(repo => repo.RetrieveAsync(spaceId))
                .ReturnsAsync(organisationSpace);

            var organisationUserSpaceRepositoryMock = new Mock<IOrganisationUserSpaceRepository>();
            organisationUserSpaceRepositoryMock.Setup(repo => repo.RetrieveAllByOrganisationSpaceIdAsync(spaceId))
                .ReturnsAsync(organisationUserSpaces);

            organisationUserSpaceRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var organisationSpaceService = new OrganisationSpaceService(organisationSpaceRepositoryMock.Object, organisationUserSpaceRepositoryMock.Object, _organisationSpaceInvitationRepository);

            // Act
            var result = await organisationSpaceService.DeleteUserFromOrganisationSpaceAsync(spaceId, userId);

            // Assert
            Assert.True(result);
            organisationUserSpaceRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DeleteUserFromOrganisationSpaceAsync_OrganisationSpaceNotFound_ThrowsOrganisationSpaceNotFoundException()
        {
            // Arrange
            string spaceId = "space-1";
            string userId = "user-1";

            var organisationSpaceRepositoryMock = new Mock<IOrganisationSpaceRepository>();
            organisationSpaceRepositoryMock.Setup(repo => repo.RetrieveAsync(spaceId))
                .ReturnsAsync((OrganisationSpace)null);

            var organisationSpaceService = new OrganisationSpaceService(organisationSpaceRepositoryMock.Object, _organisationUserSpaceRepository, _organisationSpaceInvitationRepository);

            // Act & Assert
            await Assert.ThrowsAsync<OrganisationSpaceNotFoundException>(async () =>
            {
                await organisationSpaceService.DeleteUserFromOrganisationSpaceAsync(spaceId, userId);
            });
        }

        [Fact]
        public async Task DeleteUserFromOrganisationSpaceAsync_UserNotMemberOfSpace_ThrowsException()
        {
            // Arrange
            string spaceId = "space-1";
            string userId = "user-1";

            var organisationSpace = new OrganisationSpace
            {
                Id = spaceId,
                Title = "Space 1"
            };

            var organisationUserSpaces = new List<OrganisationUserSpace>
            {
                new OrganisationUserSpace { Id = "user-space-1", OrganisationSpaceId = spaceId, UserId = "user-2" },
                new OrganisationUserSpace { Id = "user-space-2", OrganisationSpaceId = spaceId, UserId = "user-3" }
            };

            var organisationSpaceRepositoryMock = new Mock<IOrganisationSpaceRepository>();
            organisationSpaceRepositoryMock.Setup(repo => repo.RetrieveAsync(spaceId))
                .ReturnsAsync(organisationSpace);

            var organisationUserSpaceRepositoryMock = new Mock<IOrganisationUserSpaceRepository>();
            organisationUserSpaceRepositoryMock.Setup(repo => repo.RetrieveAllByOrganisationSpaceIdAsync(spaceId))
                .ReturnsAsync(organisationUserSpaces);

            var organisationSpaceService = new OrganisationSpaceService(organisationSpaceRepositoryMock.Object, organisationUserSpaceRepositoryMock.Object, _organisationSpaceInvitationRepository);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await organisationSpaceService.DeleteUserFromOrganisationSpaceAsync(spaceId, userId);
            });
        }

        #endregion

        #region GetOrganisationSpaces Test Methods

        [Fact]
        public async Task GetOrganisationSpaces_ValidUserId_ReturnsOrganisationSpaces()
        {
            // Arrange
            string userId = "user-1";

            var organisationSpaces = new List<OrganisationSpace>
            {
                new OrganisationSpace { Id = "space-1", CreatorId = userId, Title = "Space 1" },
                new OrganisationSpace { Id = "space-2", CreatorId = userId, Title = "Space 2" },
                new OrganisationSpace { Id = "space-3", CreatorId = "user-2", Title = "Space 3" },
            };

            var organisationSpaceRepositoryMock = new Mock<IOrganisationSpaceRepository>();
            organisationSpaceRepositoryMock.Setup(repo => repo.RetrieveAllByCreatorIdAsync(userId))
                .ReturnsAsync(organisationSpaces.Where(space => space.CreatorId == userId));

            var organisationSpaceService = new OrganisationSpaceService(organisationSpaceRepositoryMock.Object, _organisationUserSpaceRepository, _organisationSpaceInvitationRepository);

            // Act
            var result = await organisationSpaceService.GetOrganisationSpaces(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, space => Assert.Equal(userId, space.CreatorId));
            Assert.Contains(result, space => space.Id == "space-1");
            Assert.Contains(result, space => space.Id == "space-2");
        }

        #endregion

        #region GetUsersInOrganisationSpace Test Methods

        [Fact]
        public async Task GetUsersInOrganisationSpace_ValidOrganisationSpaceId_ReturnsUsers()
        {
            // Arrange
            string organisationSpaceId = "space-1";

            var organisationUserSpaces = new List<OrganisationUserSpace>
            {
                new OrganisationUserSpace { Id = "user-space-1", OrganisationSpaceId = organisationSpaceId, UserId = "user-1" },
                new OrganisationUserSpace { Id = "user-space-2", OrganisationSpaceId = organisationSpaceId, UserId = "user-2" },
                new OrganisationUserSpace { Id = "user-space-3", OrganisationSpaceId = "space-2", UserId = "user-1" },
            };

            var organisationUserSpaceRepositoryMock = new Mock<IOrganisationUserSpaceRepository>();
            organisationUserSpaceRepositoryMock.Setup(repo => repo.RetrieveAllByOrganisationSpaceIdAsync(organisationSpaceId))
                .ReturnsAsync(organisationUserSpaces.Where(ous => ous.OrganisationSpaceId == organisationSpaceId));

            var organisationSpaceService = new OrganisationSpaceService(_organisationSpaceRepository, organisationUserSpaceRepositoryMock.Object, _organisationSpaceInvitationRepository);

            // Act
            var result = await organisationSpaceService.GetUsersInOrganisationSpace(organisationSpaceId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, ous => Assert.Equal(organisationSpaceId, ous.OrganisationSpaceId));
            Assert.Contains(result, ous => ous.Id == "user-space-1" && ous.UserId == "user-1");
            Assert.Contains(result, ous => ous.Id == "user-space-2" && ous.UserId == "user-2");
        }

        #endregion

        #region RejectInvitationAsync

        [Fact]
        public async Task RejectInvitationAsync_ValidInvitationToken_ReturnsTrue()
        {
            // Arrange
            string invitationToken = EncryptionHelper.Encrypt("token-1", _encryptionKey);
            string decryptionKey = _encryptionKey;

            var organisationSpaceInvitation = new OrganisationSpaceInvitation
            {
                Id = 1,
                InvitationToken = "token-1",
                InvitationStatus = InvitationStatus.Pending.GetDescription()
            };

            var organisationSpaceInvitationRepositoryMock = new Mock<IOrganisationSpaceInvitationRepository>();
            organisationSpaceInvitationRepositoryMock.Setup(repo => repo.RetrieveByInvitationTokenAsync(It.IsAny<string>()))
                .ReturnsAsync(organisationSpaceInvitation);
            organisationSpaceInvitationRepositoryMock.Setup(repo => repo.UpdateAsync(organisationSpaceInvitation.Id, It.IsAny<OrganisationSpaceInvitation>()))
                .ReturnsAsync(organisationSpaceInvitation);

            var organisationSpaceService = new OrganisationSpaceService(_organisationSpaceRepository, _organisationUserSpaceRepository, organisationSpaceInvitationRepositoryMock.Object);

            // Act
            var result = await organisationSpaceService.RejectInvitationAsync(invitationToken, decryptionKey);

            // Assert
            Assert.True(result);
            organisationSpaceInvitationRepositoryMock.Verify(repo => repo.UpdateAsync(organisationSpaceInvitation.Id, organisationSpaceInvitation), Times.Once);
        }

        [Fact]
        public async Task RejectInvitationAsync_InvalidInvitationToken_ThrowsOrganisationSpaceInvitationNotFoundException()
        {
            // Arrange
            string invitationToken = EncryptionHelper.Encrypt("invalidToken", _encryptionKey);
            string decryptionKey = _encryptionKey;

            var organisationSpaceInvitationRepositoryMock = new Mock<IOrganisationSpaceInvitationRepository>();
            organisationSpaceInvitationRepositoryMock.Setup(repo => repo.RetrieveByInvitationTokenAsync(It.IsAny<string>()))
                .ReturnsAsync((OrganisationSpaceInvitation)null);

            var organisationSpaceService = new OrganisationSpaceService(_organisationSpaceRepository, _organisationUserSpaceRepository, organisationSpaceInvitationRepositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<OrganisationSpaceInvitationNotFoundException>(async () =>
            {
                await organisationSpaceService.RejectInvitationAsync(invitationToken, decryptionKey);
            });
        }

        [Fact]
        public async Task RejectInvitationAsync_UnableToUpdateInvitation_ReturnsFalse()
        {
            // Arrange
            string invitationToken = EncryptionHelper.Encrypt("token-1", _encryptionKey);
            string decryptionKey = _encryptionKey;

            var organisationSpaceInvitation = new OrganisationSpaceInvitation
            {
                Id = 1,
                InvitationToken = "token-1",
                InvitationStatus = InvitationStatus.Pending.GetDescription()
            };

            var organisationSpaceInvitationRepositoryMock = new Mock<IOrganisationSpaceInvitationRepository>();
            organisationSpaceInvitationRepositoryMock.Setup(repo => repo.RetrieveByInvitationTokenAsync(It.IsAny<string>()))
                .ReturnsAsync(organisationSpaceInvitation);
            organisationSpaceInvitationRepositoryMock.Setup(repo => repo.UpdateAsync(organisationSpaceInvitation.Id, It.IsAny<OrganisationSpaceInvitation>()))
                .ReturnsAsync((OrganisationSpaceInvitation)null);

            var organisationSpaceService = new OrganisationSpaceService(_organisationSpaceRepository, _organisationUserSpaceRepository, organisationSpaceInvitationRepositoryMock.Object);

            // Act
            var result = await organisationSpaceService.RejectInvitationAsync(invitationToken, decryptionKey);

            // Assert
            Assert.False(result);
            organisationSpaceInvitationRepositoryMock.Verify(repo => repo.UpdateAsync(organisationSpaceInvitation.Id, organisationSpaceInvitation), Times.Once);
        }

        #endregion

        #region SendInvitationAsync Test Methods

        [Fact]
        public async Task SendInvitationAsync_ValidArguments_ReturnsEncryptedToken()
        {
            // Arrange
            string spaceId = "space-1";
            string inviterId = "user-1";
            string inviteeId = "user-2";
            UserRole role = UserRole.Admin;
            string encryptionKey = _encryptionKey;

            var organisationSpace = new OrganisationSpace
            {
                Id = spaceId,
                Title = "Space 1"
            };

            var organisationSpaceInvitation = new OrganisationSpaceInvitation
            {
                InvitationToken = "token-1"
            };

            var organisationSpaceRepositoryMock = new Mock<IOrganisationSpaceRepository>();
            organisationSpaceRepositoryMock.Setup(repo => repo.RetrieveAsync(spaceId))
                .ReturnsAsync(organisationSpace);

            var organisationSpaceInvitationRepositoryMock = new Mock<IOrganisationSpaceInvitationRepository>();
            organisationSpaceInvitationRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<OrganisationSpaceInvitation>()))
                .ReturnsAsync(organisationSpaceInvitation);

            var organisationSpaceService = new OrganisationSpaceService(organisationSpaceRepositoryMock.Object, _organisationUserSpaceRepository, organisationSpaceInvitationRepositoryMock.Object);

            // Act
            var result = await organisationSpaceService.SendInvitationAsync(spaceId, inviterId, inviteeId, role, encryptionKey);

            // Assert
            Assert.NotNull(result);
            organisationSpaceRepositoryMock.Verify(repo => repo.RetrieveAsync(spaceId), Times.Once);
            organisationSpaceInvitationRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<OrganisationSpaceInvitation>()), Times.Once);
        }

        [Fact]
        public async Task SendInvitationAsync_InvalidSpaceId_ThrowsOrganisationSpaceNotFoundException()
        {
            // Arrange
            string spaceId = "invalid-space-id";
            string inviterId = "user-1";
            string inviteeId = "user-2";
            UserRole role = UserRole.Admin;
            string encryptionKey = _encryptionKey;

            var organisationSpaceRepositoryMock = new Mock<IOrganisationSpaceRepository>();
            organisationSpaceRepositoryMock.Setup(repo => repo.RetrieveAsync(spaceId))
                .ReturnsAsync((OrganisationSpace)null);

            var organisationSpaceService = new OrganisationSpaceService(organisationSpaceRepositoryMock.Object, _organisationUserSpaceRepository, _organisationSpaceInvitationRepository);

            // Act & Assert
            await Assert.ThrowsAsync<OrganisationSpaceNotFoundException>(async () =>
            {
                await organisationSpaceService.SendInvitationAsync(spaceId, inviterId, inviteeId, role, encryptionKey);
            });
        }

        [Fact]
        public async Task SendInvitationAsync_InvitationNotSent_ThrowsOrganisationSpaceInvitationNotSentException()
        {
            // Arrange
            string spaceId = "space-1";
            string inviterId = "user-1";
            string inviteeId = "user-2";
            UserRole role = UserRole.Admin;
            string encryptionKey = _encryptionKey;

            var organisationSpace = new OrganisationSpace
            {
                Id = spaceId,
                Title = "Space 1"
            };

            var organisationSpaceRepositoryMock = new Mock<IOrganisationSpaceRepository>();
            organisationSpaceRepositoryMock.Setup(repo => repo.RetrieveAsync(spaceId))
                .ReturnsAsync(organisationSpace);

            var organisationSpaceInvitationRepositoryMock = new Mock<IOrganisationSpaceInvitationRepository>();
            organisationSpaceInvitationRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<OrganisationSpaceInvitation>()))
                .ReturnsAsync((OrganisationSpaceInvitation)null);

            var organisationSpaceService = new OrganisationSpaceService(organisationSpaceRepositoryMock.Object, _organisationUserSpaceRepository, organisationSpaceInvitationRepositoryMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<OrganisationSpaceInvitationNotSentException>(async () =>
            {
                await organisationSpaceService.SendInvitationAsync(spaceId, inviterId, inviteeId, role, encryptionKey);
            });
        }

        #endregion

        #region UpdateOrganisationSpaceAsync Test Methods

        [Fact]
        public async Task UpdateOrganisationSpaceAsync_ValidArguments_ReturnsUpdatedOrganisationSpace()
        {
            // Arrange
            string spaceId = "space-1";
            string newTitle = "Updated Space";
            string newDescription = "Updated description";

            var organisationSpace = new OrganisationSpace
            {
                Id = spaceId,
                Title = "Space 1",
                Description = "Initial description"
            };

            var organisationSpaceRepositoryMock = new Mock<IOrganisationSpaceRepository>();
            organisationSpaceRepositoryMock.Setup(repo => repo.RetrieveAsync(spaceId))
                .ReturnsAsync(organisationSpace);
            organisationSpaceRepositoryMock.Setup(repo => repo.UpdateAsync(spaceId, It.IsAny<OrganisationSpace>()))
                .ReturnsAsync(organisationSpace);

            var organisationSpaceService = new OrganisationSpaceService(organisationSpaceRepositoryMock.Object, _organisationUserSpaceRepository, _organisationSpaceInvitationRepository);

            // Act
            var result = await organisationSpaceService.UpdateOrganisationSpaceAsync(spaceId, newTitle, newDescription);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newTitle, result.Title);
            Assert.Equal(newDescription, result.Description);
            organisationSpaceRepositoryMock.Verify(repo => repo.RetrieveAsync(spaceId), Times.Once);
            organisationSpaceRepositoryMock.Verify(repo => repo.UpdateAsync(spaceId, It.IsAny<OrganisationSpace>()), Times.Once);
        }

        [Fact]
        public async Task UpdateOrganisationSpaceAsync_InvalidSpaceId_ThrowsOrganisationSpaceNotFoundException()
        {
            // Arrange
            string spaceId = "invalid-space-id";
            string newTitle = "Updated Space";
            string newDescription = "Updated description";

            var organisationSpaceRepositoryMock = new Mock<IOrganisationSpaceRepository>();
            organisationSpaceRepositoryMock.Setup(repo => repo.RetrieveAsync(spaceId))
                .ReturnsAsync((OrganisationSpace)null);

            var organisationSpaceService = new OrganisationSpaceService(organisationSpaceRepositoryMock.Object, _organisationUserSpaceRepository, _organisationSpaceInvitationRepository);

            // Act & Assert
            await Assert.ThrowsAsync<OrganisationSpaceNotFoundException>(async () =>
            {
                await organisationSpaceService.UpdateOrganisationSpaceAsync(spaceId, newTitle, newDescription);
            });
        }

        #endregion

        #region UpdateUserRoleAsync Test Methods

        [Fact]
        public async Task UpdateUserRoleAsync_ValidArguments_ReturnsUpdatedOrganisationUserSpace()
        {
            // Arrange
            string spaceId = "space-1";
            string userId = "user-1";
            UserRole newRole = UserRole.Admin;

            var organisationSpace = new OrganisationSpace
            {
                Id = spaceId,
                Title = "Space 1"
            };

            var organisationUserSpace = new OrganisationUserSpace
            {
                Id = "user-space-1",
                OrganisationSpaceId = spaceId,
                UserId = userId,
                RoleId = (int)UserRole.Member
            };

            var organisationSpaceRepositoryMock = new Mock<IOrganisationSpaceRepository>();
            organisationSpaceRepositoryMock.Setup(repo => repo.RetrieveAsync(spaceId))
                .ReturnsAsync(organisationSpace);

            var organisationUserSpaceRepositoryMock = new Mock<IOrganisationUserSpaceRepository>();
            organisationUserSpaceRepositoryMock.Setup(repo => repo.RetrieveAllByOrganisationSpaceIdAsync(spaceId))
                .ReturnsAsync(new List<OrganisationUserSpace> { organisationUserSpace });
            organisationUserSpaceRepositoryMock.Setup(repo => repo.UpdateAsync(organisationUserSpace))
                .ReturnsAsync(organisationUserSpace);

            var organisationSpaceService = new OrganisationSpaceService(organisationSpaceRepositoryMock.Object, organisationUserSpaceRepositoryMock.Object, _organisationSpaceInvitationRepository);

            // Act
            var result = await organisationSpaceService.UpdateUserRoleAsync(spaceId, userId, newRole);

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)newRole, result.RoleId);
            organisationSpaceRepositoryMock.Verify(repo => repo.RetrieveAsync(spaceId), Times.Once);
            organisationUserSpaceRepositoryMock.Verify(repo => repo.RetrieveAllByOrganisationSpaceIdAsync(spaceId), Times.Once);
            organisationUserSpaceRepositoryMock.Verify(repo => repo.UpdateAsync(organisationUserSpace), Times.Once);
        }

        [Fact]
        public async Task UpdateUserRoleAsync_InvalidSpaceId_ThrowsOrganisationSpaceInvitationNotFoundException()
        {
            // Arrange
            string spaceId = "invalid-space-id";
            string userId = "user-1";
            UserRole newRole = UserRole.Admin;

            var organisationSpaceRepositoryMock = new Mock<IOrganisationSpaceRepository>();
            organisationSpaceRepositoryMock.Setup(repo => repo.RetrieveAsync(spaceId))
                .ReturnsAsync((OrganisationSpace)null);

            var organisationSpaceService = new OrganisationSpaceService(organisationSpaceRepositoryMock.Object, _organisationUserSpaceRepository, _organisationSpaceInvitationRepository);

            // Act & Assert
            await Assert.ThrowsAsync<OrganisationSpaceNotFoundException>(async () =>
            {
                await organisationSpaceService.UpdateUserRoleAsync(spaceId, userId, newRole);
            });
        }

        [Fact]
        public async Task UpdateUserRoleAsync_UserNotMember_ThrowsException()
        {
            // Arrange
            string spaceId = "space_id - 1";
            string userId = "user-48";
            UserRole newRole = UserRole.Admin;

            var organisationSpace = new OrganisationSpace
            {
                Id = spaceId,
                Title = "Space 1"
            };

            var organisationUserSpaceRepositoryMock = new Mock<IOrganisationUserSpaceRepository>();
            organisationUserSpaceRepositoryMock.Setup(repo => repo.RetrieveAllByOrganisationSpaceIdAsync(spaceId))
                .ReturnsAsync(new List<OrganisationUserSpace>());

            var organisationSpaceService = new OrganisationSpaceService(_organisationSpaceRepository, organisationUserSpaceRepositoryMock.Object, _organisationSpaceInvitationRepository);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await organisationSpaceService.UpdateUserRoleAsync(spaceId, userId, newRole);
            });
        }


        #endregion
    }
}
