namespace Forum.Domain.Entities
{
    public class EntityBase
    {
        public long Id { get; set; }
        public bool Active { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
