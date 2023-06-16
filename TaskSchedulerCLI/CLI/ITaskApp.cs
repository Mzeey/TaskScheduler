using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskSchedulerCLI.CLI
{
    public interface ITaskApp
    {
        Task<bool> CreateTask();
        Task EditTask();
        Task DeleteTask();
        Task ViewTaskDetails();

        Task ViewMyTasks();
    }
}
