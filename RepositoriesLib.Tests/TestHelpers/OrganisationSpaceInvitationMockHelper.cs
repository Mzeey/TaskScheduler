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
    public class OrganisationSpaceInvitationMockHelper : MockHelper<IOrganisationSpaceInvitationRepository>
    {
        public OrganisationSpaceInvitationMockHelper()
        {
            _repositoryMock = new Mock<IOrganisationSpaceInvitationRepository>();
        }
        public override Mock<IOrganisationSpaceInvitationRepository> ConfigureRepositoryMock()
        {
            var invitations = GenerateData<OrganisationSpaceInvitation>(10);

            _repositoryMock.Setup(repo => repo.RetrieveAllAsync())
                .ReturnsAsync(() => invitations);

            _repositoryMock.Setup(repo => repo.RetrieveAllByInviterIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string inviterId) => invitations.Where(i => i.InviterId.ToUpper() == inviterId.ToUpper()));

            _repositoryMock.Setup(repo => repo.RetrieveAllByInviteeIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string inviteeId) => invitations.Where(i => i.InviteeId.ToUpper() == inviteeId.ToUpper()));

            _repositoryMock.Setup(repo => repo.RetrieveAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => invitations.FirstOrDefault(i => i.Id == id));

            _repositoryMock.Setup(repo => repo.RetrieveByInvitationTokenAsync(It.IsAny<string>()))
                .ReturnsAsync((string invitationToken) => invitations.FirstOrDefault(i => i.InvitationToken.ToUpper() == invitationToken.ToUpper()));

            _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<OrganisationSpaceInvitation>()))
                .ReturnsAsync((OrganisationSpaceInvitation invitation) =>
                {
                    invitation.Id = invitations.Count + 1;
                    invitation.CreatedDate = DateTime.UtcNow;
                    invitation.UpdatedDate = DateTime.UtcNow;
                    invitation.InvitationStatus = "Pending";

                    invitations.Add(invitation);
                    return invitation;
                });

            _repositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<OrganisationSpaceInvitation>()))
                .ReturnsAsync((int id, OrganisationSpaceInvitation invitation) =>
                {
                    if (id != invitation.Id)
                        return null;
                    var existingInvitation = invitations.FirstOrDefault(i => i.Id == id);
                    if (existingInvitation is null)
                        return null ;
                    existingInvitation = invitation;
                    return existingInvitation;
                });

            _repositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    var existingInvitation = invitations.FirstOrDefault(i => i.Id == id);
                    if (existingInvitation is null)
                        return false;
                    invitations.Remove(existingInvitation);
                    return true;
                });


            return _repositoryMock;
        }

        protected override List<T> GenerateData<T>(int count)
        {
            var invitations = new List<T>();
            for (int i = 1; i <= count; i++)
            {
                var invitation = new OrganisationSpaceInvitation
                {
                    Id = i,
                    CreatedDate = DateTime.Now,
                    InvitationStatus = "Status",
                    InvitationToken = $"token-{i}",
                    InviteeId = $"invitee-{i}",
                    InviterId = $"inviter-{i}",
                    RoleId = i,
                    OrganisationSpaceId = $"space-{i}",
                    UpdatedDate = DateTime.UtcNow,
                };

                invitations.Add((T)(object)invitation);
            }
            return invitations;
        }
    }
}
