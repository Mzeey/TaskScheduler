using Mzeey.Entities;
using Mzeey.SharedLib.Enums;
using Mzeey.UserManagementLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSchedulerCLI.CLI
{
    internal class UserApp : IUserApp
    {
        private readonly IUserService _userService;
        private string _authorizationToken;
        public UserApp(IUserService userService)
        {
            _userService = userService;
        }
        public void ChangePassword()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateAccount()
        {
            bool isCreated;
            Console.WriteLine("\n--- Create Account ---");

            Console.Write("Enter First Name: ");
            string firstName = Console.ReadLine().Trim();
            
            Console.Write("Enter last Name: ");
            string lastName = Console.ReadLine().Trim();

            Console.Write("Enter username: ");
            string username = Console.ReadLine().Trim();

            Console.Write("Enter password: ");
            string password = Console.ReadLine().Trim();

            Console.Write("Enter email: ");
            string email = Console.ReadLine().Trim();

            UserRole role = UserRole.User;
            User user = new User();
            try
            {
                user = await _userService.CreateUserAsync(firstName, lastName, username, password, email, role);
                isCreated = (user != null) ?  true : false;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                isCreated = false;
            }
            return isCreated;
        }

        public async Task<bool> DeleteAccount()
        {
            throw new NotImplementedException();
        }

        public void EditProfile()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Login()
        {
            Console.WriteLine("\n--- Login ---");

            Console.WriteLine("Enter username: ");
            string username = Console.ReadLine().Trim();

            Console.WriteLine("Enter password: ");
            string password = Console.ReadLine().Trim();

            bool isLoggedin;
            try
            {
                string authToken = await _userService.LoginAsync(username, password);
                isLoggedin = (authToken != null)? true : false;
                _authorizationToken = authToken;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                isLoggedin = false;
            }

            return isLoggedin;
        }

        public async Task<bool> Logout()
        {
            bool isLoggedOut = await _userService.LogoutAsync(_authorizationToken);
            return isLoggedOut;
        }

        public void ViewProfileDetails()
        {
            throw new NotImplementedException();
        }
    }
}
