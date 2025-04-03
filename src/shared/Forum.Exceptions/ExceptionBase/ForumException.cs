using System.Net;

namespace Forum.Exceptions.ExceptionBase
{
    public abstract class ForumException(string message) : System.Exception(message)
    {
        public abstract IList<string> GetErrorMessage();
        public abstract HttpStatusCode GetHttpStatusCode();
    }
}
