namespace Forum.Domain.Entities
{
    public class Comment : EntityBase
    {
        public string Content { get; set; } = string.Empty;
        public long TopicId { get; set; }
        public long UserId { get; set; }

        public Topic? Topic { get; set; }
        public User? User { get; set; }
    }
}
