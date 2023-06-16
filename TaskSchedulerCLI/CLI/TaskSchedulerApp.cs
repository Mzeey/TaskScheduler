using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Mzeey.TaskSchedulerLib.Services;
using Mzeey.UserManagementLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSchedulerCLI.CLI
{
    internal class TaskSchedulerApp
    {
        private bool _isExitRequested = false;
        private bool _isLoggedIn =false;


        private readonly IUserService _userService;
        private readonly ITaskService _taskService;

        private readonly IMenuHandler _menuHandler;
        private readonly IUserApp _userApp;
        private readonly ITaskApp _taskApp;

        public TaskSchedulerApp(IUserService userService, ITaskService taskService)
        {
            _userService = userService;
            _taskService = taskService;

            _menuHandler = new MenuHandler();
            _userApp = new UserApp(_userService);
            _taskApp = new TaskApp(_taskService, _userService);
        }

        public void Run()
        {
            Console.WriteLine("Welcome to the Task Scheduler!");
            while (!_isExitRequested)
            {
                if (!_isLoggedIn)
                {
                    _isLoggedIn = ProcessLoginMenu();
                }
                else
                {
                    bool exitRequested = ProcessMainMenu().GetAwaiter().GetResult();
                    if (exitRequested)
                        break;
                }
                
            }

            Console.WriteLine("Thank you for choosing TaskScheduler Goodbye!");
        }

        private bool ProcessLoginMenu()
        {
            _menuHandler.DisplayLoginMenu();

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Task<bool> isCreated = _userApp.CreateAccount();
                    if (isCreated.Result)
                    {
                        Console.WriteLine("Account created successfully. Please login.");
                    }
                    return false;
                case "2":
                    Task<bool> loggedIn = _userApp.Login();
                    if (loggedIn.Result)
                        Console.WriteLine("Login successful!");
                    else
                        Console.WriteLine("Invalid credentials. Please try again.");
                    return loggedIn.Result;
                case "3":
                    _isExitRequested = true;
                    return false;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    return false;
            }
        }

        private async Task<bool> ProcessMainMenu()
        {
            bool loggedIn = _isLoggedIn;

            _menuHandler.DisplayMainMenu();

            string input = Console.ReadLine();
            input = input.Trim();
            switch (input)
            {
                case "1":
                    await _taskApp.CreateTask();
                    break;
                case "2":
                     await _taskApp.EditTask();
                    break;
                case "3":
                     await _taskApp.DeleteTask();
                    break;
                case "4":
                    await _taskApp.ViewTaskDetails();
                    break;
                case "5":
                    await _taskApp.ViewMyTasks();
                    break;
                case "6":
                     _userApp.EditProfile();
                    break;
                case "7":
                     _userApp.ChangePassword();
                    break;
                case "8":
                     _userApp.DeleteAccount();
                    loggedIn = false;
                    break;
                case "9":
                     _userApp.ViewProfileDetails();
                    break;
                case "10":
                     _userApp.Logout();
                    loggedIn = false;
                    break;
                case "11":
                    _isExitRequested = true;
                    _isLoggedIn = false;
                    return true;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            _isLoggedIn = loggedIn;
            _isExitRequested = false;
            return false;
        }


    }
}
