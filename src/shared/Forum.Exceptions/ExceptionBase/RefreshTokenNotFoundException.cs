using System.Net;

namespace Forum.Exceptions.ExceptionBase
{
    public class RefreshTokenNotFoundException : ForumException
    {
        public RefreshTokenNotFoundException() : base(ResourceMessagesException.TOKEN_NOT_FOUND)
        {
        }

        public override IList<string> GetErrorMessage() => [Message];

        public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.Unauthorized;
    }
}
