using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.SharedLib.Exceptions
{
    public class AuthenticationTokenNotCreatedException : Exception
    {
        public AuthenticationTokenNotCreatedException(string userId) :base($"Authentication Token Could not be created for user: '{userId}'"){ }
        public AuthenticationTokenNotCreatedException(string userId, string message): base(message) { }
        public AuthenticationTokenNotCreatedException(string userId, string message, Exception innerException): base(message, innerException) { }
    }
}
