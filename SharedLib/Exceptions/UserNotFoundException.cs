using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.SharedLib.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string userId)
            : base($"User with id: {userId} was not found")
        { }

        public UserNotFoundException(string userId, string message)
            : base(message)
        { }

        public UserNotFoundException(string userId, string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
