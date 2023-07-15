using Microsoft.IdentityModel.Tokens;
using Mzeey.Entities;
using Mzeey.Repositories;
using Mzeey.SharedLib.Enums;
using Mzeey.SharedLib.Exceptions;
using Mzeey.SharedLib.Extensions;
using Mzeey.SharedLib.Utilities;
using NHibernate.Id.Insert;
using NHibernate.Linq.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using TaskSchedulerLib.Services;

namespace Mzeey.TaskSchedulerLib.Services
{
    public class OrganisationSpaceService : IOrganisationSpaceService
    {
        private readonly IOrganisationSpaceRepository _organisationSpaceRepository;
        private readonly IOrganisationUserSpaceRepository _organisationUserSpaceRepository;
        private readonly IOrganisationSpaceInvitationRepository _organisationSpaceInvitationRepository;

        public OrganisationSpaceService(IOrganisationSpaceRepository organisationSpaceRepository, IOrganisationUserSpaceRepository organisationUserSpaceRepository, IOrganisationSpaceInvitationRepository organisationSpaceInvitationRepository)
        {
            _organisationSpaceRepository = organisationSpaceRepository;
            _organisationUserSpaceRepository = organisationUserSpaceRepository;
            _organisationSpaceInvitationRepository = organisationSpaceInvitationRepository;
        }

        public async Task<bool> AcceptInvitationAsync(string invitationToken, string decryptionKey)
        {
            string decryptedInvitationToken = decryptInvitationToken(invitationToken, decryptionKey);
            OrganisationSpaceInvitation organisationSpaceInvitation = await _organisationSpaceInvitationRepository.RetrieveByInvitationTokenAsync(decryptedInvitationToken);
            if(organisationSpaceInvitation is null)
            {
                throw new OrganisationSpaceInvitationNotFoundException(invitationToken);
            }

            organisationSpaceInvitation.InvitationStatus = InvitationStatus.Accepted.GetDescription();
            organisationSpaceInvitation.UpdatedDate = DateTime.UtcNow;

            organisationSpaceInvitation = await _organisationSpaceInvitationRepository.UpdateAsync(organisationSpaceInvitation.Id, organisationSpaceInvitation);
            if(organisationSpaceInvitation is null)
            {
                throw new Exception("Unable to accept Invitation");
            }

            OrganisationUserSpace organisationUserSpace = new OrganisationUserSpace
            {
                Id = UniqueIdGenerator.GenerateUniqueId(),
                OrganisationSpaceId = organisationSpaceInvitation.OrganisationSpaceId,
                UserId = organisationSpaceInvitation.InviteeId,
                RoleId = organisationSpaceInvitation.RoleId,
            };

            organisationUserSpace = await _organisationUserSpaceRepository.CreateAsync(organisationUserSpace);
            if(organisationUserSpace is null)
            {
                organisationSpaceInvitation.InvitationStatus = InvitationStatus.Pending.GetDescription();
                organisationSpaceInvitation.UpdatedDate = DateTime.UtcNow;

                organisationSpaceInvitation = await _organisationSpaceInvitationRepository.UpdateAsync(organisationSpaceInvitation.Id, organisationSpaceInvitation);

                return false;
            }

            return true;
        }

        public async Task<OrganisationSpace> CreateOrganisationSpaceAsync(string userId, string title, string description, OrganisationSpaceType spaceType)
        {
            validateCreateOrganisationSpaceArguments(userId, title);

            OrganisationSpace organisationSpace = await createOrganisationSpaceEntityAsync(userId, title, description, spaceType);

            OrganisationUserSpace organisationUserSpace = await createOrganisationUserSpaceAsync(organisationSpace, userId);

            return organisationSpace;
        }

        public async Task<bool> DeleteOrganisationSpaceAsync(string spaceId)
        {
            OrganisationSpace organisationSpace = await retrieveOrganisationSpaceAsync(spaceId);
            List<OrganisationUserSpace> organisationSpaceMembers = await retrieveOrganisationSpaceMembersAsync(spaceId); 

            await deleteOrganisationSpaceMembersAsync(organisationSpaceMembers);

            bool organisationSpaceDeleted = await deleteOrganisationSpaceAsync(organisationSpace);

            if (!organisationSpaceDeleted)
            {
                await restoreOrganisationSpaceMembersAsync(organisationSpaceMembers);
            }

            return organisationSpaceDeleted;
        }

        public async Task<bool> DeleteUserFromOrganisationSpaceAsync(string spaceId, string userId)
        {
            OrganisationSpace organisationSpace = await _organisationSpaceRepository.RetrieveAsync(spaceId);
            if(organisationSpace is null)
            {
                throw new OrganisationSpaceNotFoundException(spaceId);
            }

            IEnumerable<OrganisationUserSpace> organisationUserSpaces = await _organisationUserSpaceRepository.RetrieveAllByOrganisationSpaceIdAsync(organisationSpace.Id);
            OrganisationUserSpace organisationUserSpace = organisationUserSpaces.FirstOrDefault(ous => ous.UserId.ToUpper() == userId.ToUpper());
            if(organisationUserSpace is null)
            {
                throw new Exception($"User with Id '{userId}' is not a memeber of '{organisationSpace.Title}' Space");
            }

            bool organisationUserSpaceDeleted = await _organisationUserSpaceRepository.DeleteAsync(organisationUserSpace.Id);

            return organisationUserSpaceDeleted;
        }

        public async Task<IEnumerable<OrganisationSpace>> GetOrganisationSpaces(string userId)
        {
            return await _organisationSpaceRepository.RetrieveAllByCreatorIdAsync(userId);
        }

        public async Task<IEnumerable<OrganisationUserSpace>> GetUsersInOrganisationSpace(string organsationSpaceId)
        {
            return await _organisationUserSpaceRepository.RetrieveAllByOrganisationSpaceIdAsync(organsationSpaceId);
        }

        public async Task<bool> RejectInvitationAsync(string invitationToken, string decryptionKey)
        {
            string decryptedToken = decryptInvitationToken(invitationToken, decryptionKey);
            OrganisationSpaceInvitation organisationSpaceInvitation = await _organisationSpaceInvitationRepository.RetrieveByInvitationTokenAsync(decryptedToken);

            if (organisationSpaceInvitation is null)
                throw new OrganisationSpaceInvitationNotFoundException(invitationToken);

            organisationSpaceInvitation.InvitationStatus = InvitationStatus.Rejected.GetDescription();
            organisationSpaceInvitation =  await _organisationSpaceInvitationRepository.UpdateAsync(organisationSpaceInvitation.Id, organisationSpaceInvitation);

            if (organisationSpaceInvitation is null)
                return false;

            return true;
        }

        public async Task<string> SendInvitationAsync(string spaceId, string inviterId, string inviteeId, UserRole role, string encryptionKey)
        {
            OrganisationSpace organisationSpace = await _organisationSpaceRepository.RetrieveAsync(spaceId);
            if(organisationSpace is null)
            {
                throw new OrganisationSpaceNotFoundException(spaceId);
            }

            string Invitationtoken = generateInvitationToken();
            OrganisationSpaceInvitation organisationSpaceInvitation = new OrganisationSpaceInvitation
            {
                InvitationStatus = InvitationStatus.Pending.GetDescription(),
                InviteeId = inviteeId,
                InviterId = inviterId,
                InvitationToken = Invitationtoken,
                OrganisationSpaceId = organisationSpace.Id,
                RoleId = (int) role,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };
            organisationSpaceInvitation = await _organisationSpaceInvitationRepository.CreateAsync(organisationSpaceInvitation);
            if(organisationSpaceInvitation is null)
            {
                throw new OrganisationSpaceInvitationNotSentException(organisationSpace.Title);
            }
            string encryptedToken = encryptInvitationToken(Invitationtoken, encryptionKey);

            return encryptedToken;
        }

        public async Task<OrganisationSpace> UpdateOrganisationSpaceAsync(string spaceId, string title, string description)
        {
            OrganisationSpace organisationSpace = await _organisationSpaceRepository.RetrieveAsync(spaceId);
            if(organisationSpace is null)
            {
                throw new OrganisationSpaceNotFoundException(spaceId);
            }

            organisationSpace.Title = title;
            organisationSpace.Description = description;

            organisationSpace = await _organisationSpaceRepository.UpdateAsync(organisationSpace.Id, organisationSpace);
            return organisationSpace;
        }

        public async Task<OrganisationUserSpace> UpdateUserRoleAsync(string spaceId, string userId, UserRole role)
        {
            OrganisationSpace organisationSpace = await _organisationSpaceRepository.RetrieveAsync(spaceId);
            if(organisationSpace is null)
            {
                throw new OrganisationSpaceNotFoundException(spaceId);
            }

            IEnumerable<OrganisationUserSpace> organisationUserSpaces = await _organisationUserSpaceRepository.RetrieveAllByOrganisationSpaceIdAsync(organisationSpace.Id);
            OrganisationUserSpace organisationUserSpace = organisationUserSpaces.FirstOrDefault(ous => ous.UserId.ToUpper() == userId.ToUpper());
            if(organisationUserSpace is null)
            {
                throw new Exception($"User with Id '{userId}' is not a memeber of '{organisationSpace.Title}' Space");
            }
            organisationUserSpace.RoleId = (int)role;
            

            return await _organisationUserSpaceRepository.UpdateAsync(organisationUserSpace);
        }

        private string encryptInvitationToken(string token, string encryptionKey)
        {
            return EncryptionHelper.Encrypt(token, encryptionKey);
        }

        private string decryptInvitationToken(string token, string decryptionKey)
        {
            return EncryptionHelper.Decrypt(token, decryptionKey);
        }

        private string generateInvitationToken()
        {
            return UniqueIdGenerator.GenerateUniqueId();
        }

        #region CreateOrganisationSpaceAsync Helper Functions
        private void validateCreateOrganisationSpaceArguments(string userId, string title)
        {
            if (userId is null)
            {
                throw new ArgumentException("User Id is null");
            }

            if (title is null)
            {
                throw new ArgumentException("Title is null");
            }
        }

        private async Task<OrganisationSpace> createOrganisationSpaceEntityAsync(string userId, string title, string description, OrganisationSpaceType spaceType)
        {
            OrganisationSpace organisationSpace = new OrganisationSpace
            {
                Id = UniqueIdGenerator.GenerateUniqueId(),
                Title = title,
                CreatorId = userId,
                Description = description,
                IsPrivate = (int)spaceType == 1 ? true : false
            };

            organisationSpace = await _organisationSpaceRepository.CreateAsync(organisationSpace);

            if (organisationSpace is null)
            {
                throw new OrganisationSpaceNotCreatedException(title);
            }

            return organisationSpace;
        }

        private async Task<OrganisationUserSpace> createOrganisationUserSpaceAsync(OrganisationSpace organisationSpace, string userId)
        {
            OrganisationUserSpace organisationUserSpace = new OrganisationUserSpace
            {
                Id = UniqueIdGenerator.GenerateUniqueId(),
                OrganisationSpaceId = organisationSpace.Id,
                UserId = userId,
                RoleId = (int)UserRole.Admin
            };

            organisationUserSpace = await _organisationUserSpaceRepository.CreateAsync(organisationUserSpace);

            if (organisationUserSpace is null)
            {
                await deleteOrganisationSpaceAsync(organisationSpace);
                throw new OrganisationSpaceNotCreatedException(organisationSpace.Title);
            }

            return organisationUserSpace;
        }

        private async Task<bool> deleteOrganisationSpaceAsync(OrganisationSpace organisationSpace)
        {
            return await _organisationSpaceRepository.DeleteAsync(organisationSpace.Id);
        }
        #endregion

        #region DeleteOrganisationSpaceAsync Helper Functions
        private async Task<OrganisationSpace> retrieveOrganisationSpaceAsync(string spaceId)
        {
            OrganisationSpace organisationSpace = await _organisationSpaceRepository.RetrieveAsync(spaceId);
            if (organisationSpace is null)
            {
                throw new OrganisationSpaceNotFoundException(spaceId);
            }

            return organisationSpace;
        }

        private async Task<List<OrganisationUserSpace>> retrieveOrganisationSpaceMembersAsync(string spaceId)
        {
            List<OrganisationUserSpace> organisationSpaceMembers = (await _organisationUserSpaceRepository.RetrieveAllByOrganisationSpaceIdAsync(spaceId)).ToList();
            return organisationSpaceMembers;
        }

        private async Task deleteOrganisationSpaceMembersAsync(List<OrganisationUserSpace> organisationSpaceMembers)
        {
            var deleteTasks = organisationSpaceMembers.Select(spaceMember => _organisationUserSpaceRepository.DeleteAsync(spaceMember.Id));
            await Task.WhenAll(deleteTasks);
        }

        private async Task restoreOrganisationSpaceMembersAsync(List<OrganisationUserSpace> organisationSpaceMembers)
        {
            var restoreTasks = organisationSpaceMembers.Select(spaceMember => _organisationUserSpaceRepository.CreateAsync(spaceMember));
            await Task.WhenAll(restoreTasks);
        }
        #endregion
    }
}
