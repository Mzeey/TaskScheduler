using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.SharedLib.Exceptions
{
    public class InvalidLoginCredentialsException : Exception
    {
        public InvalidLoginCredentialsException() : base ("Invalid Login Credentials Provided")
        {
        }

        public InvalidLoginCredentialsException(string? message) : base(message)
        {
        }

        public InvalidLoginCredentialsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidLoginCredentialsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
