using System.ComponentModel;

namespace Mzeey.SharedLib.Enums
{
    public enum UserRole
    {
        [Description("Admin")]
        Admin = 1,
        [Description("Manager")]
        Manager = 2,
        [Description("User")]
        User = 3,
    }
}
