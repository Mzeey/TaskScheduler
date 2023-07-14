using Microsoft.IdentityModel.Tokens;
using Mzeey.Entities;
using Mzeey.Repositories;
using Mzeey.SharedLib.Enums;
using Mzeey.SharedLib.Exceptions;
using Mzeey.SharedLib.Utilities;
using NHibernate.Linq.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Task<bool> AcceptInvitationAsync(string invitationToken, string decryptionKey)
        {
            throw new NotImplementedException();
        }

        public async Task<OrganisationSpace> CreateOrganisationSpaceAsync(string userId, string title, string description, OrganisationSpaceType spaceType)
        {
            if(userId is null)
            {
                throw new ArgumentException("User Id is null");
            }

            if(title is null)
            {
                throw new ArgumentException("Title is null");
            }

            OrganisationSpace organisationSpace = new OrganisationSpace
            {
                Id = UniqueIdGenerator.GenerateUniqueId(),
                Title = title,
                CreatorId = userId,
                Description = description,
                IsPrivate = (int)spaceType == 1 ? true : false
            };

            organisationSpace = await _organisationSpaceRepository.CreateAsync(organisationSpace);
            if(organisationSpace is null)
            {
                throw new OrganisationSpaceNotCreatedException(title);
            }

            OrganisationUserSpace organisationUserSpace = new OrganisationUserSpace
            {
                Id = UniqueIdGenerator.GenerateUniqueId(),
                OrganisationSpaceId = organisationSpace.Id,
                UserId = userId,
                RoleId = (int)UserRole.Admin
            };

            organisationUserSpace = await _organisationUserSpaceRepository.CreateAsync(organisationUserSpace);
            if(organisationUserSpace is null)
            {
                await _organisationSpaceRepository.DeleteAsync(organisationSpace.Id);
                throw new OrganisationSpaceNotCreatedException(title);
            }

            return organisationSpace;
        }

        public async Task<bool> DeleteOrganisationSpaceAsync(string spaceId)
        {
            OrganisationSpace organisationSpace = await RetrieveOrganisationSpaceAsync(spaceId);
            List<OrganisationUserSpace> organisationSpaceMembers = await RetrieveOrganisationSpaceMembersAsync(spaceId);

            await DeleteOrganisationSpaceMembersAsync(organisationSpaceMembers);

            bool organisationSpaceDeleted = await DeleteOrganisationSpaceAsync(organisationSpace);

            if (!organisationSpaceDeleted)
            {
                await RestoreOrganisationSpaceMembersAsync(organisationSpaceMembers);
            }

            return organisationSpaceDeleted;
        }

        public Task<bool> DeleteUserFromOrganisationSpaceAsync(string spaceId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrganisationSpace>> GetOrganisationSpaces(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrganisationUserSpace>> GetUsersInOrganisationSpace(string organsationSpaceId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RejectInvitationAsync(string invitationToken, string decryptionKey)
        {
            throw new NotImplementedException();
        }

        public Task<string> SendInvitationAsync(string spaceId, string inviterId, string inviteeId, UserRole role, string encryptionKey)
        {
            throw new NotImplementedException();
        }

        public Task<OrganisationSpace> UpdateOrganisationSpaceAsync(string spaceId, string title, string description)
        {
            throw new NotImplementedException();
        }

        public Task<OrganisationUserSpace> UpdateUserRoleAsync(string spaceId, string userId, UserRole role)
        {
            throw new NotImplementedException();
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

        #region DeleteOrganisationSpaceAsync Helper Functions
        private async Task<OrganisationSpace> RetrieveOrganisationSpaceAsync(string spaceId)
        {
            OrganisationSpace organisationSpace = await _organisationSpaceRepository.RetrieveAsync(spaceId);
            if (organisationSpace is null)
            {
                throw new OrganisationSpaceNotFoundException(spaceId);
            }

            return organisationSpace;
        }

        private async Task<List<OrganisationUserSpace>> RetrieveOrganisationSpaceMembersAsync(string spaceId)
        {
            List<OrganisationUserSpace> organisationSpaceMembers = (await _organisationUserSpaceRepository.RetrieveByOrganisationSpaceIdAsync(spaceId)).ToList();
            return organisationSpaceMembers;
        }

        private async Task DeleteOrganisationSpaceMembersAsync(List<OrganisationUserSpace> organisationSpaceMembers)
        {
            var deleteTasks = organisationSpaceMembers.Select(spaceMember => _organisationUserSpaceRepository.DeleteAsync(spaceMember.Id));
            await Task.WhenAll(deleteTasks);
        }

        private async Task<bool> DeleteOrganisationSpaceAsync(OrganisationSpace organisationSpace)
        {
            bool organisationSpaceDeleted = await _organisationSpaceRepository.DeleteAsync(organisationSpace.Id);
            return organisationSpaceDeleted;
        }

        private async Task RestoreOrganisationSpaceMembersAsync(List<OrganisationUserSpace> organisationSpaceMembers)
        {
            var restoreTasks = organisationSpaceMembers.Select(spaceMember => _organisationUserSpaceRepository.CreateAsync(spaceMember));
            await Task.WhenAll(restoreTasks);
        }
        #endregion
    }
}
