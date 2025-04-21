namespace Forum.Communication.Response
{
    public class ResponseTopicDetailsJson
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }

        public ResponseUserSummaryJson User { get; set; } = default!;

        public int TotalLikes { get; set; }
        public bool LikedByCurrentUser { get; set; }

        public List<ResponseCommentJson> Comments { get; set; } = [];
        public int TotalComments { get; set; }
    }
}
