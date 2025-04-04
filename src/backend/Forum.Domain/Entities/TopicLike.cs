namespace Forum.Domain.Entities
{
    public class TopicLike
    {
        public long UserId { get; set; }
        public long TopicId { get; set; }

        public User? User { get; set; }
        public Topic? Topic { get; set; }
    }
}
