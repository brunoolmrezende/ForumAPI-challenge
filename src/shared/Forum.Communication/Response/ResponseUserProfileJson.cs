namespace Forum.Communication.Response
{
    public class ResponseUserProfileJson
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public int TopicsCount { get; set; }
        public int CommentsCount { get; set; }
    }
}
