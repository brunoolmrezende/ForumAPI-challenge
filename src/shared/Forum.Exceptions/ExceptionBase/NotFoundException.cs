using System.Net;

namespace Forum.Exceptions.ExceptionBase
{
    public class NotFoundException(string message) : ForumException(message)
    {
        public override IList<string> GetErrorMessage() => [Message];

        public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.NotFound;
    }
}
