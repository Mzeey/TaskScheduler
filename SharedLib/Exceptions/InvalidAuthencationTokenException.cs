using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.SharedLib.Exceptions
{
    public class InvalidAuthenticationTokenException : Exception
    {
        public InvalidAuthenticationTokenException(string authenticationToken): base($"The authentication token '{authenticationToken}' is Invalid")
        {
        }

        public InvalidAuthenticationTokenException(string authenticationToken, string message) : base(message)
        {
        }

        public InvalidAuthenticationTokenException(string authenticationToken, string message, Exception innerException) : base(message, innerException)
        {
        }
        }
    }
}
