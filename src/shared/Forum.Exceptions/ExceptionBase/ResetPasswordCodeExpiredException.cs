using System.Net;

namespace Forum.Exceptions.ExceptionBase
{
    public class ResetPasswordCodeExpiredException : ForumException
    {
        public ResetPasswordCodeExpiredException() : base(ResourceMessagesException.EXPIRED_CODE)
        {
        }

        public override IList<string> GetErrorMessage() => [Message];

        public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.BadRequest;
    }
}
