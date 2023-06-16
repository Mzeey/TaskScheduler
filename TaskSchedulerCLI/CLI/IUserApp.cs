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
        void EditProfile();
        void ChangePassword();
        void DeleteAccount();
        void ViewProfileDetails();
        void Logout();
    }
}
