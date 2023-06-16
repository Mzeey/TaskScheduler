
using System.ComponentModel;

namespace Mzeey.SharedLib.Enums
{
    public enum TaskItemStatus
    {
        [Description("Pending")]
        Pending,

        [Description("In Progress")]
        InProgress,

        [Description("Completed")]
        Completed,

        [Description("Cancelled")]
        Cancelled
    }
}
