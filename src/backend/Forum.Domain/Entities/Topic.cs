namespace Forum.Domain.Entities
{
    public class Topic : EntityBase
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public long UserId { get; set; }

        public ICollection<Comment> Comments { get; set; } = [];
    }
}
