using Mzeey.Entities;
using Mzeey.Repositories;
using Mzeey.SharedLib.Exceptions;
using Mzeey.SharedLib.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementLib.Services
{
    //Todo: Write Test Class for these
    public class AuthenticationService : IAuthenticationService
    {
        private IAuthenticationTokenRepository _authenticationTokenRepository;
        private IUserRepository _userRepository;

        public AuthenticationService(IAuthenticationTokenRepository authenticationTokenRepository, IUserRepository userRepository)
        {
            _authenticationTokenRepository = authenticationTokenRepository;
            _userRepository = userRepository;
        }
        public async Task<bool> DeleteAuthenticationToken(string authenticationToken, string decryptionKey)
        {
            if (string.IsNullOrEmpty(authenticationToken))
            {
                throw new ArgumentException("Authentication Token is null");
            }

            if (string.IsNullOrEmpty(decryptionKey))
            {
                throw new ArgumentException("Decryption Key is null");
            }

            string decryptedToken = decryptAuthenticationToken(authenticationToken, decryptionKey);

            AuthenticationToken existingAuthenticationToken = await _authenticationTokenRepository.RetrieveByTokenAsync(decryptedToken);

            if (existingAuthenticationToken == null)
            {
                throw new InvalidAuthenticationTokenException(authenticationToken);
            }

            return await _authenticationTokenRepository.DeleteAsync(existingAuthenticationToken.Id);
        }

        public async Task<string> GenerateAuthenticationToken(string userId, string encryptionKey)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User Id is null");
            }

            if (string.IsNullOrEmpty(encryptionKey)){
                throw new ArgumentException("Encryption Key is null");
            }

            User user = await _userRepository.RetrieveAsync(userId);

            if(user is null)
            {
                throw new UserNotFoundException(userId);
            }

            DateTime currentDate = DateTime.Now;
            string token = generateUniqueToken();
            const int token_Validity_Period_Extension_In_Days = 14;

            AuthenticationToken authenticationToken = new AuthenticationToken {
                IssuedDate = currentDate,
                Token = token,
                ExpirationDate = currentDate.AddDays(token_Validity_Period_Extension_In_Days),
                UserId = userId
            };

            AuthenticationToken addedAuthenticationtoken = await _authenticationTokenRepository.CreateAsync(authenticationToken);
            if(addedAuthenticationtoken is null)
            {
                throw new AuthenticationTokenNotCreatedException(userId);
            }

            return encryptAuthenticationToken(token, encryptionKey);
        }

        public async Task<User> ValidateAuthenticationToken(string authenticationToken, string decryptionKey)
        {
            if (string.IsNullOrEmpty(authenticationToken))
            {
                throw new ArgumentException("Authentication Token is null");
            }

            if (string.IsNullOrEmpty(decryptionKey))
            {
                throw new ArgumentException("Decryption Key is null");
            }

            string decryptedAuthenticationToken = decryptAuthenticationToken(authenticationToken, decryptionKey);
            AuthenticationToken exisitingAuthenticationToken = await _authenticationTokenRepository.RetrieveByTokenAsync(decryptedAuthenticationToken);
            if (exisitingAuthenticationToken is null)
            {
                throw new InvalidAuthenticationTokenException(authenticationToken);
            }

            User user = await _userRepository.RetrieveAsync(exisitingAuthenticationToken.UserId);

            if(user is null)
            {
                throw new UserNotFoundException(exisitingAuthenticationToken.UserId, $"User could not be found with the authentication token '{authenticationToken}'");
            }

            return user;
        }

        private string decryptAuthenticationToken(string authenticationToken, string decryptionKey)
        {
            return EncryptionHelper.Decrypt(authenticationToken, decryptionKey);
        }

        private string encryptAuthenticationToken(string authenticationToken, string encryptionKey)
        {
            return EncryptionHelper.Encrypt(authenticationToken, encryptionKey);
        }

        private string generateUniqueToken()
        {
            return UniqueIdGenerator.GenerateUniqueId();
        }
    }
}
