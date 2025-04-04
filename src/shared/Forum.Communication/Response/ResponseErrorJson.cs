namespace Forum.Communication.Response
{
    public class ResponseErrorJson
    {
        public IList<string> Errors { get; set; }
        public bool TokenIsExpired { get; set; }

        public ResponseErrorJson(IList<string> errors)
        {
            Errors = errors;
        }

        public ResponseErrorJson(string error)
        {
            Errors = [error];
        }
    }
}
