using System.Net;

namespace Forum.Exceptions.ExceptionBase
{
    public class ResetPasswordCodeNotFoundException : ForumException
    {
        public ResetPasswordCodeNotFoundException() : base(ResourceMessagesException.CODE_NOT_FOUND)
        {
        }

        public override IList<string> GetErrorMessage() => [Message];

        public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.NotFound;
    }
}
