using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mzeey.Entities;
using Mzeey.SharedLib.Enums;
using Mzeey.SharedLib.Utilities;
using Mzeey.UserManagementLib.Repositories;
using Mzeey.UserManagementLib.Utilities;

namespace Mzeey.UserManagementLib.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationTokenRepository _tokenRepository;
        private string _loggedInUserId;
        public string GetLoggedInUserId()
        {
            return _loggedInUserId;
        }

        public UserService(IUserRepository userRepository, IAuthenticationTokenRepository tokenRepository)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
        }

        public async Task<User> CreateUserAsync(string firstName, string lastName, string username, string password, string email, UserRole userRole)
        {
            // Check if a user with the same username already exists
            User existingUser = await _userRepository.RetrieveByUserNameAsync(username);
            if (existingUser != null)
            {
                throw new ArgumentException("Username already exists");
            }
            existingUser = await _userRepository.RetrieveByEmailAsync(email);
            if(existingUser != null)
            {
                throw new ArgumentException("Email already Exist");
            }

            // Generate a salt and hash the password
            string salt = PasswordHasher.GenerateSalt();
            string hashedPassword = PasswordHasher.HashPassword(password, salt);

            // Create the user entity
            User newUser = new User
            {
                Id = UniqueIdGenerator.GenerateUniqueId(),
                FirstName = firstName,
                LastName = lastName,
                Username = username,
                Password = hashedPassword,
                Salt = salt,
                Email = email,
                RoleId = (int) userRole
            };

            return await _userRepository.CreateAsync(newUser);
        }

        public async Task<User> UpdateUserAsync(string userId, string firstName, string lastName, string email)
        {
            User existingUser = await _userRepository.RetrieveAsync(userId);
            if (existingUser == null)
            {
                return null; // User not found
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

        public async Task<User> GetUserAsync(string userId)
        {
            return await _userRepository.RetrieveAsync(userId);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.RetrieveByUserNameAsync(username);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.RetrieveAllAsync();
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            // Retrieve the user by username
            User user = await _userRepository.RetrieveByUserNameAsync(username);
            if (user == null)
            {
                throw new ArgumentException("Invalid username");
            }

            // Verify the password
            bool isPasswordValid = PasswordHasher.VerifyPassword(password, user.Salt, user.Password);
            if (!isPasswordValid)
            {
                throw new ArgumentException("Invalid password");
            }

            _loggedInUserId = getUserIDByUsername(username).Result;

            // Generate an authentication token
            string token = UniqueIdGenerator.GenerateUniqueId();

            // Create the authentication token entity
            AuthenticationToken authToken = new AuthenticationToken
            {
                UserId = user.Id,
                Token = token,
                IssuedDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(7) // Set the token expiration to 7 days
            };

            // Save the authentication token
            await _tokenRepository.CreateAsync(authToken);

            return token;
        }

        private async Task<string> getUserIDByUsername(string username)
        {
            User user = await _userRepository.RetrieveByUserNameAsync(username);
            return user.Id;
        }

        public async Task<bool> LogoutAsync(string token)
        {
            // Retrieve the authentication token by token value
            AuthenticationToken authToken = await _tokenRepository.RetrieveByTokenAsync(token);
            if (authToken == null)
            {
                return false; // Token not found
            }
            _loggedInUserId = string.Empty;

            // Delete the authentication token
            return await _tokenRepository.DeleteAsync(authToken.TokenId);
        }

        public async Task<User> ChangeUserRoleAsync(string userId, UserRole newRole)
        {
            User user = await _userRepository.RetrieveAsync(userId);

            if (user != null)
            {
                user.RoleId = (int) newRole;

                return await _userRepository.UpdateAsync(userId, user);
            }

            return null;
        }

    }
}
