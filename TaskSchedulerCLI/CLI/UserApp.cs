using Mzeey.Entities;
using Mzeey.SharedLib.Enums;
using Mzeey.SharedLib.Extensions;
using Mzeey.UserManagementLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSchedulerCLI.CLI
{
    //Temporarily disabling this service
    internal class UserApp // : IUserApp
    {
        //private readonly IUserService _userService;
        //private string _authorizationToken;
        //public UserApp(IUserService userService)
        //{
        //    _userService = userService;
        //}
        //public async Task ChangePassword()
        //{
        //    Console.WriteLine("\n--- Change Password ---");

        //    Console.Write("Enter current password: ");
        //    string currentPassword = Console.ReadLine().Trim();

        //    Console.Write("Enter new password: ");
        //    string newPassword = Console.ReadLine().Trim();

        //    try
        //    {
        //        User user = await _userService.ChangeUserPassword(_userService.GetLoggedInUserId(), currentPassword, newPassword);
        //        Console.WriteLine("Password changed successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Failed to change password: " + ex.Message);
        //    }
        //}

        //public async Task<bool> CreateAccount()
        //{
        //    bool isCreated;
        //    Console.WriteLine("\n--- Create Account ---");

        //    Console.Write("Enter First Name: ");
        //    string firstName = Console.ReadLine().Trim();
            
        //    Console.Write("Enter last Name: ");
        //    string lastName = Console.ReadLine().Trim();

        //    Console.Write("Enter username: ");
        //    string username = Console.ReadLine().Trim();

        //    Console.Write("Enter password: ");
        //    string password = Console.ReadLine().Trim();

        //    Console.Write("Enter email: ");
        //    string email = Console.ReadLine().Trim();

        //    UserRole role = UserRole.User;
        //    User user = new User();
        //    try
        //    {
        //        user = await _userService.CreateUserAsync(firstName, lastName, username, password, email, role);
        //        isCreated = (user != null) ?  true : false;
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        isCreated = false;
        //    }
        //    return isCreated;
        //}

        //public async Task<bool> DeleteAccount()
        //{
        //    Console.WriteLine("\n--- Delete Account ---");
        //    Console.WriteLine("Are you sure you want to delete your account? (Y/N)");
        //    string input = Console.ReadLine();

        //    if (input.ToUpper() == "Y")
        //    {
        //        bool isDeleted = await _userService.DeleteUserAsync(_userService.GetLoggedInUserId());
        //        if (isDeleted)
        //        {
        //            Console.WriteLine("Account deleted successfully. Goodbye!");
        //            return true;
        //        }
        //        else
        //        {
        //            Console.WriteLine("Failed to delete account.");
        //        }
        //    }
        //    else if (input.ToUpper() == "N")
        //    {
        //        Console.WriteLine("Account deletion canceled.");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Invalid choice. Account deletion canceled.");
        //    }

        //    return false;
        //}

        //public async Task EditProfile()
        //{
        //    Console.WriteLine("\n--- Edit Profile ---");

        //    // Retrieve the logged-in user's profile details
        //    User user = await _userService.GetUserAsync(_userService.GetLoggedInUserId());
        //    if (user != null)
        //    {
        //        Console.WriteLine($"Username: {user.Username}");
        //        Console.WriteLine($"First Name: {user.FirstName}");
        //        Console.WriteLine($"Last Name: {user.LastName}");
        //        Console.WriteLine($"Email: {user.Email}");

        //        Console.WriteLine("Enter new information (leave blank to keep the current value):");

        //        Console.Write("First Name: ");
        //        string newFirstName = Console.ReadLine().Trim();
        //        newFirstName = (string.IsNullOrWhiteSpace(newFirstName))? user.FirstName: newFirstName;

        //        Console.Write("Last Name: ");
        //        string newLastName = Console.ReadLine().Trim();
        //        newLastName = (string.IsNullOrWhiteSpace(newLastName)) ? user.LastName : newLastName;

        //        Console.Write("Email: ");
        //        string newEmail = Console.ReadLine().Trim();
        //        newEmail = (string.IsNullOrWhiteSpace(newEmail)) ? user.Email : newEmail;

        //        user = await _userService.UpdateUserAsync(user.Id, newFirstName, newLastName, newEmail);
        //        if (user != null)
        //        {
        //            Console.WriteLine("Profile updated successfully.");
        //        }
        //        else
        //        {
        //            Console.WriteLine("Failed to update profile.");
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("Failed to retrieve profile details.");
        //    }
        //}


        //public async Task<bool> Login()
        //{
        //    Console.WriteLine("\n--- Login ---");

        //    Console.WriteLine("Enter username: ");
        //    string username = Console.ReadLine().Trim();

        //    Console.WriteLine("Enter password: ");
        //    string password = Console.ReadLine().Trim();

        //    bool isLoggedin;
        //    try
        //    {
        //        string authToken = await _userService.LoginAsync(username, password);
        //        isLoggedin = (authToken != null)? true : false;
        //        _authorizationToken = authToken;
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        isLoggedin = false;
        //    }

        //    return isLoggedin;
        //}

        //public async Task<bool> Logout()
        //{
        //    bool isLoggedOut = await _userService.LogoutAsync(_authorizationToken);
        //    return isLoggedOut;
        //}

        //public async Task ViewProfileDetails()
        //{
        //    Console.WriteLine("\n--- Profile Details ---");

        //    // Retrieve the logged-in user's profile details
        //    User user = await _userService.GetUserAsync(_userService.GetLoggedInUserId());
        //    if (user != null)
        //    {
        //        Console.WriteLine($"Username: {user.Username}");
        //        Console.WriteLine($"First Name: {user.FirstName}");
        //        Console.WriteLine($"Last Name: {user.LastName}");
        //        Console.WriteLine($"Email: {user.Email}");
        //        //UserRole role = (UserRole) user.RoleId;
        //        //Console.WriteLine($"Role: {role.GetDescription()}");

        //        // Additional profile details can be displayed here

        //        Console.WriteLine("Profile details displayed.");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Failed to retrieve profile details.");
        //    }
        //}
    }
}
