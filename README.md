# Task Scheduler Solution
Welcome to the Task Scheduler Solution, a comprehensive task scheduling system that includes a command-line interface (CLI) and supporting libraries.
## Overview
The Task Scheduler Solution allows users to manage their tasks and accounts through the CLI. It provides functionality to create, edit, and delete tasks, as well as manage user accounts and profiles. The solution is built using the C# programming language and .NET Core framework.
## Solution Structure
The solution is organized into multiple projects within the solution file. Here's an overview of the project structure:
* `TaskSchedulerCLI`: The command-line interface (CLI) project that provides the user interface for interacting with the Task Scheduler system.
* `TaskSchedulerLib`: The core library project that contains the business logic and services for managing tasks and user accounts.
* `UserManagementLib`: A library project that handles user management functionality, including account creation, login, and profile management.
* `TaskSchedulerDb`: A project that manages the database operations using Entity Framework Core. It includes the database context, migrations, and repository implementations.
## Installation
To install and run the Task Scheduler Solution, follow these steps:
1. Clone the repository:
```
git clone [https://github.com/your-username/task-scheduler-solution.git](https://github.com/Mzeey/TaskScheduler.git)
```
2. Open the solution file (`TaskScheduler.sln`) in your preferred development environment (e.g., Visual Studio, Visual Studio Code).
3. Build the solution to restore the dependencies.
4. Update the connection string in the `appsettings.json` file of the `TaskSchedulerDb` project to point to your SQL Server database.
5. Run the database migrations to create the necessary tables:

```
dotnet ef database update --project TaskSchedulerDb
```
6. Set the `TaskSchedulerCLI` project as the startup project.
7. Run the solution to start the Task Scheduler CLI:
```
dotnet run --project TaskSchedulerCLI
```
## Usage
* When prompted, select an option from the menu by entering the corresponding number.
* Follow the instructions provided by the CLI to create a user account, login, manage tasks, and perform other actions.
## Contributing
Contributions to the Task Scheduler Solution are welcome! If you find any issues or have suggestions for improvements, please create a new issue or submit a pull request.

## License
This project is licensed under the MIT License.
