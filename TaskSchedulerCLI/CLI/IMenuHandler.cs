using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSchedulerCLI.CLI
{
    public interface IMenuHandler
    {
        void DisplayMainMenu();
        void DisplayLoginMenu();
    }
}
