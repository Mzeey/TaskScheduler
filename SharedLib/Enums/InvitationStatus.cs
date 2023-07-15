using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.SharedLib.Enums
{
    public enum InvitationStatus
    {
        [Description("Pending")]
        Pending,
        [Description("Accepted")]
        Accepted,
        [Description("Rejected")]
        Rejected
    }
}
