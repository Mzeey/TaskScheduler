
using Mzeey.TaskSchedulerLib.Services;
using Mzeey.Repositories;
using Mzeey.UserManagementLib.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using Mzeey.DbContextLib;
using TaskSchedulerCLI.CLI;

namespace TaskSchedulerCLI;
class Program
{
    static void Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // Get the connection string from the configuration
        var connectionString = config.GetConnectionString("DefaultConnection");

        // Create DbContextOptions using the connection string
        var optionsBuilder = new DbContextOptionsBuilder<TaskSchedulerContext>()
            .UseSqlServer(connectionString);

        var dbContext = new TaskSchedulerContext(optionsBuilder.Options);

        IUserRepository userRepository = new UserRepository(dbContext);
        ITaskRepository taskRepository = new TaskRepository(dbContext);
        IAuthenticationTokenRepository authenticationTokenRepository = new AuthenticationTokenRepository(dbContext);

        IUserService userService = new UserService(userRepository, authenticationTokenRepository);
        ITaskService taskService = new TaskService(taskRepository);
        TaskSchedulerApp app = new TaskSchedulerApp(userService, taskService);

        app.Run();
        
    }
}