namespace Forum.Domain.Entities
{
    public class CommentLike : EntityBase
    {
        public long UserId { get; set; }
        public long CommentId { get; set; }

        public User User { get; set; } = default!;
        public Comment Comment { get; set; } = default!;
    }
}
