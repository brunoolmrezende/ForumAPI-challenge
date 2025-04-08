namespace Forum.Domain.Entities
{
    public class TopicLike : EntityBase
    {
        public long UserId { get; set; }
        public long TopicId { get; set; }

        public User User { get; set; } = default!;
        public Topic Topic { get; set; } = default!;
    }
}
