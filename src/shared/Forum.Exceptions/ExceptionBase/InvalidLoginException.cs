using System.Net;

namespace Forum.Exceptions.ExceptionBase
{
    public class InvalidLoginException : ForumException
    {
        public InvalidLoginException() : base(ResourceMessagesException.INVALID_EMAIL_OR_PASSWORD)
        {
        }

        public override IList<string> GetErrorMessage() => [Message];

        public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.Unauthorized;
    }
}
