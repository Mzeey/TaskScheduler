using Moq;
using Mzeey.Entities;
using Mzeey.Repositories;
using RepositoriesLib.Tests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RepositoriesLib.Tests.Repositories
{
    public class OrganisationSpaceInvitationRepositoryTests
    {
        private readonly IMockHelper<IOrganisationSpaceInvitationRepository> _mockHelper;
        private readonly IOrganisationSpaceInvitationRepository _organisationSpaceInvitationRepository;

        public OrganisationSpaceInvitationRepositoryTests()
        {
            _mockHelper = new OrganisationSpaceInvitationMockHelper();
            _organisationSpaceInvitationRepository = _mockHelper.ConfigureRepositoryMock().Object;
        }

        [Fact]
        public async Task CreateAsync_Should_Add_OrganisationSpaceInvitation_To_Repository()
        {
            // Arrange
            var newInvitation = new OrganisationSpaceInvitation
            {
                InviterId = "inviter-id",
                InviteeId = "invitee-id",
                OrganisationSpaceId = "space-id",
                RoleId = 1
            };

            // Act
            var createdInvitation = await _organisationSpaceInvitationRepository.CreateAsync(newInvitation);

            // Assert
            Assert.NotNull(createdInvitation);
            Assert.NotEqual(0, createdInvitation.Id);
            Assert.Equal(newInvitation.InviterId, createdInvitation.InviterId);
            Assert.Equal(newInvitation.InviteeId, createdInvitation.InviteeId);
            Assert.Equal(newInvitation.OrganisationSpaceId, createdInvitation.OrganisationSpaceId);
            Assert.Equal(newInvitation.RoleId, createdInvitation.RoleId);
        }

        [Fact]
        public async Task UpdateAsync_ExistingInvitationId_ReturnsUpdatedInvitation()
        {
            // Arrange
            var invitationId = 1;
            var existingInvitation = await _organisationSpaceInvitationRepository.RetrieveAsync(invitationId);

            Assert.NotNull(existingInvitation);

            // Update the invitation
            var updatedInvitation = new OrganisationSpaceInvitation
            {
                Id = invitationId,
                InvitationStatus = "UpdatedStatus"
            };

            // Act
            var result = await _organisationSpaceInvitationRepository.UpdateAsync(invitationId, updatedInvitation);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedInvitation.InvitationStatus, result.InvitationStatus);
        }

        [Fact]
        public async Task RetrieveAllAsync_ReturnsAllOrganisationSpaceInvitations()
        {
            // Act
            var invitations = await _organisationSpaceInvitationRepository.RetrieveAllAsync();

            // Assert
            Assert.NotNull(invitations);
            Assert.Equal(10, invitations.Count());
        }

        [Fact]
        public async Task RetrieveAllByInviterIdAsync_ExistingInviterId_ReturnsInvitationsOfInviter()
        {
            // Arrange
            var inviterId = "inviter-3";

            // Act
            var invitations = await _organisationSpaceInvitationRepository.RetrieveAllByInviterIdAsync(inviterId);

            // Assert
            Assert.NotNull(invitations);
            Assert.NotEmpty(invitations);
            Assert.All(invitations, invitation => Assert.Equal(inviterId, invitation.InviterId));
        }

        [Fact]
        public async Task RetrieveAllByInviteeIdAsync_ExistingInviteeId_ReturnsInvitationsOfInvitee()
        {
            // Arrange
            var inviteeId = "invitee-4";

            // Act
            var invitations = await _organisationSpaceInvitationRepository.RetrieveAllByInviteeIdAsync(inviteeId);

            // Assert
            Assert.NotNull(invitations);
            Assert.NotEmpty(invitations);
            Assert.All(invitations, invitation => Assert.Equal(inviteeId, invitation.InviteeId));
        }

        [Fact]
        public async Task RetrieveAsync_ExistingInvitationId_ReturnsExistingInvitation()
        {
            // Arrange
            var invitationId = 1;

            // Act
            var existingInvitation = await _organisationSpaceInvitationRepository.RetrieveAsync(invitationId);

            // Assert
            Assert.NotNull(existingInvitation);
            Assert.Equal(invitationId, existingInvitation.Id);
        }

        [Fact]
        public async Task RetrieveByInvitationTokenAsync_ExistingInvitationToken_ReturnsExistingInvitation()
        {
            // Arrange
            var invitationToken = "token-1";

            // Act
            var existingInvitation = await _organisationSpaceInvitationRepository.RetrieveByInvitationTokenAsync(invitationToken);

            // Assert
            Assert.NotNull(existingInvitation);
            Assert.Equal(invitationToken, existingInvitation.InvitationToken);
        }

        [Fact]
        public async Task DeleteAsync_ExistingInvitationId_DeletesInvitation()
        {
            // Arrange
            var invitationId = 1;
            var existingInvitation = await _organisationSpaceInvitationRepository.RetrieveAsync(invitationId);

            // Act
            var isDeleted = await _organisationSpaceInvitationRepository.DeleteAsync(invitationId);

            // Assert
            Assert.True(isDeleted);
        }
    }
}
