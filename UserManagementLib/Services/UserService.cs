using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mzeey.Entities;
using Mzeey.SharedLib.Enums;
using Mzeey.SharedLib.Utilities;
using Mzeey.Repositories;
using Mzeey.SharedLib.Exceptions;
using UserManagementLib.Services;
using NHibernate.Mapping.ByCode.Impl;

namespace Mzeey.UserManagementLib.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;

        public UserService(IUserRepository userRepository, IAuthenticationService authenticationService)
        {
            _userRepository = userRepository;
            _authenticationService = authenticationService;
        }

        public async Task<User> RegisterUserAsync(string firstName, string lastName, string username, string password, string email)
        {
            User existingUser = await _userRepository.RetrieveByUserNameAsync(username);
            
            if(existingUser != null)
            {
                throw new Exception($"Username: '{username}' is already in use");
            }

            existingUser = await _userRepository.RetrieveByEmailAsync(email);

            if(existingUser != null)
            {
                throw new Exception($"Email: '{email}' is already registered with us");
            }

            string salt = PasswordHasher.GenerateSalt();
            string hashedPassword = PasswordHasher.HashPassword(password, salt);

            User newUser = new User
            {
                Id = UniqueIdGenerator.GenerateUniqueId(),
                FirstName = firstName,
                LastName = lastName,
                Username = username,
                Password = hashedPassword,
                Email = email,
                IsEmailVerified = false,
                LastLoginDate = null,
                Salt = salt
            };

            return await _userRepository.CreateAsync(newUser);
        }

        public async Task<string> LoginUserAsync(string username, string password, string encryptionKey)
        {
            User user = await _userRepository.RetrieveByUserNameAsync(username);

            if (user == null)
            {
                throw new InvalidLoginCredentialsException();
            }

            bool passwordValid = PasswordHasher.VerifyPassword(password, user.Salt, user.Password);
            if (!passwordValid)
            {
                throw new InvalidLoginCredentialsException();
            }

            string token;

            try
            {
                token = await _authenticationService.GenerateAuthenticationToken(user.Id, encryptionKey);
            }
            catch
            {
                throw new LoginFailedException();
            }

            return token;
        }


        public async Task<bool> LogoutUserAsync(string authenticationToken, string decryptionKey)
        {
            bool tokenDeleted;

            try
            {
                tokenDeleted = await _authenticationService.DeleteAuthenticationToken(authenticationToken, decryptionKey);
            }
            catch (Exception ex) {
                tokenDeleted = false;
            }

            return tokenDeleted;
        }


        #region User_Data_Management_Functions
        public async Task<User> GetUserAsync(string userId)
        {
            User user = await _userRepository.RetrieveAsync(userId);
            if(user is null)
            {
                throw new UserNotFoundException(userId);
            }

            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            User user = await _userRepository.RetrieveByUserNameAsync(username);
            if(user is null)
            {
                throw new UserNotFoundException(username, $"User with the username: '{username}' does not exist");
            }

            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.RetrieveAllAsync();
        }

        public async Task<User> UpdateUserAsync(string userId, string firstName, string lastName, string email)
        {
            User existingUser = await _userRepository.RetrieveAsync(userId);
            if (existingUser == null)
            {
                throw new UserNotFoundException(userId);
            }

            List<User> users = (await _userRepository.RetrieveAllAsync()).ToList();
            bool emailInUse = users.Any(u => u.Id.ToUpper() != userId.ToUpper() && u.Email.ToUpper() == email.ToUpper() );
            if (emailInUse)
            {
                throw new ArgumentException($"Email: '{email}' is already in use");
            }

            existingUser.FirstName = firstName;
            existingUser.LastName = lastName;
            existingUser.Email = email;

            return await _userRepository.UpdateAsync(userId, existingUser);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            return await _userRepository.DeleteAsync(userId);
        }

        #endregion




        #region Password_Management_Functions
        public async Task<User> ChangeUserPasswordAsync(string userId, string oldpassword, string newPassword)
        {
            User user = await _userRepository.RetrieveAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }

            bool passwordMatch = PasswordHasher.VerifyPassword(oldpassword, user.Salt, user.Password);
            if (!passwordMatch)
            {
                throw new ArgumentException("Invalid current Password");
            }

            newPassword = PasswordHasher.HashPassword(newPassword, user.Salt);

            user.Password = newPassword;
            return await _userRepository.UpdateAsync(userId, user);
        }

        //To be implemented with the notification service and emailProviderService
        public Task<string> RequestPasswordResetAsync(string email, string encryptionKey)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ResetPasword(string resetToken, string decryptionKey)
        {
            throw new NotImplementedException();
        }

        #endregion

        private string encryptPasswordResetToken(string password, string encryptionKey)
        {
            return EncryptionHelper.Encrypt(password, encryptionKey);
        }

        private string descryptPasswordResetToken(string passwordResetToken, string decryptionKey)
        {
            return EncryptionHelper.Decrypt(passwordResetToken, decryptionKey);
        }
    }
}
