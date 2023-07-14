using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.SharedLib.Exceptions
{
    public class OrganisationSpaceNotCreatedException : Exception
    {
        public OrganisationSpaceNotCreatedException(string title): base($"Organisation Space: '{title}', could not be created")
        {
        }

        public OrganisationSpaceNotCreatedException(string title, string? message) : base(message)
        {
        }

        public OrganisationSpaceNotCreatedException(string title, string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected OrganisationSpaceNotCreatedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
