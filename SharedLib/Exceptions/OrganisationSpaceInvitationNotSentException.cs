using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.SharedLib.Exceptions
{
    public class OrganisationSpaceInvitationNotSentException : Exception
    {
        public OrganisationSpaceInvitationNotSentException(string organisationSpaceTitle):  base($"Invitation to Space '{organisationSpaceTitle}' Could not be sent")
        {
        }

        public OrganisationSpaceInvitationNotSentException(string organisationSpaceTitle, string? message) : base(message)
        {
        }

        public OrganisationSpaceInvitationNotSentException(string organisationSpaceTitle, string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected OrganisationSpaceInvitationNotSentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
