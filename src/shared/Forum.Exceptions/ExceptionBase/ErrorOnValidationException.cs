using System.Net;

namespace Forum.Exceptions.ExceptionBase
{
    public class ErrorOnValidationException(IList<string> errors) : ForumException(string.Empty)
    {
        private readonly IList<string> _errors = errors;

        public override IList<string> GetErrorMessage() => _errors;

        public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.BadRequest;
    }
}
