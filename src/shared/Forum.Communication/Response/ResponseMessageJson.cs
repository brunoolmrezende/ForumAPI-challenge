namespace Forum.Communication.Response
{
    public class ResponseMessageJson
    {
        public ResponseMessageJson(string genericMessage)
        {
            Message = genericMessage;
        }

        public string Message { get; set; } = string.Empty;
    }
}
