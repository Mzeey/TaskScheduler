using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.SharedLib.Exceptions
{
    public class OrganisationSpaceNotFoundException : Exception
    {
        public OrganisationSpaceNotFoundException(string spaceId): base($"Organisation Space with Id: '{spaceId}' was not found")
        {
        }

        public OrganisationSpaceNotFoundException(string spaceId, string? message) : base(message)
        {
        }

        public OrganisationSpaceNotFoundException(string spaceId, string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected OrganisationSpaceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
