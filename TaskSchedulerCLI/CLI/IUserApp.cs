using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSchedulerCLI.CLI
{
    public interface IUserApp
    {
        Task<bool> CreateAccount();
        Task<bool> Login();
        Task EditProfile();
        Task ChangePassword();
        Task<bool> DeleteAccount();
        Task ViewProfileDetails();
        Task<bool> Logout();
    }
}
