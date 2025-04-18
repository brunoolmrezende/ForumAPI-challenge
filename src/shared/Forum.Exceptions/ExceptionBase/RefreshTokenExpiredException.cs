using System.Net;

namespace Forum.Exceptions.ExceptionBase
{
    public class RefreshTokenExpiredException : ForumException
    {
        public RefreshTokenExpiredException() : base(ResourceMessagesException.EXPIRED_TOKEN)
        {
        }

        public override IList<string> GetErrorMessage() => [Message];

        public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.Unauthorized;
    }
}
