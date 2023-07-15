using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.SharedLib.Exceptions
{
    public class OrganisationSpaceInvitationNotFoundException : Exception
    {
        public OrganisationSpaceInvitationNotFoundException(string invitationToken): base($"Organisation Space with InvitationToken : '{invitationToken}', was not found")
        {
        }

        public OrganisationSpaceInvitationNotFoundException(string invitationToken, string? message) : base(message)
        {
        }

        public OrganisationSpaceInvitationNotFoundException(string invitationToken, string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected OrganisationSpaceInvitationNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
