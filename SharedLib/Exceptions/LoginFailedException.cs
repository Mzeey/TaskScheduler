using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.SharedLib.Exceptions
{
    public class LoginFailedException : Exception
    {
        public LoginFailedException() : base("An unexpected error occured durring the login process")
        {
        }

        public LoginFailedException(string? message) : base(message)
        {
        }

        public LoginFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected LoginFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
