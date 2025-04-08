namespace Forum.Communication.Response
{
    public class ResponseCommentJson
    {
        public long Id { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
    }
}
