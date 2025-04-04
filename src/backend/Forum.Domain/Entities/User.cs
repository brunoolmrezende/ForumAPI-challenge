namespace Forum.Domain.Entities
{
    public class User : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Guid UserIdentifier { get; set; }

        public IList<Topic> Topics { get; set; } = [];
        public IList<Comment> Comments { get; set; } = [];
    }
}
