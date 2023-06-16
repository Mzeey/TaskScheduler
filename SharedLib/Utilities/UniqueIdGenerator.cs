using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.SharedLib.Utilities
{
    public static class UniqueIdGenerator
    {
        public static string GenerateUniqueId()
        {
            Guid uniqueId = Guid.NewGuid();
            return uniqueId.ToString();
        }
    }
}
