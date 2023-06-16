using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSchedulerCLI.CLI
{
    public class MenuHandler : IMenuHandler
    {
        public void DisplayLoginMenu()
        {
            Console.WriteLine("\n1. Create Account");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice: ");
        }

        public void DisplayMainMenu()
        {
            Console.WriteLine("\n--- Menu ---");
            Console.WriteLine("1. Create Task");
            Console.WriteLine("2. Edit Task");
            Console.WriteLine("3. Delete Task");
            Console.WriteLine("4. View Task Details");
            Console.WriteLine("5. View My Tasks"); // New option to view user's tasks
            Console.WriteLine("6. Edit Profile");
            Console.WriteLine("7. Change Password");
            Console.WriteLine("8. Delete Account");
            Console.WriteLine("9. View Profile Details");
            Console.WriteLine("10. Logout");
            Console.WriteLine("11. Exit");
            Console.Write("Enter your choice: ");
        }
    }
}
