namespace Forum.Domain.Entities
{
    public class Comment : EntityBase
    {
        public string Content { get; set; } = string.Empty;
        public long TopicId { get; set; }
        public long UserId { get; set; }

        public Topic Topic { get; set; } = default!;
        public User User { get; set; } = default!;
        public List<CommentLike> Likes { get; set; } = [];
    }
}
