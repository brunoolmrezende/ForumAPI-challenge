namespace Forum.Domain.Entities
{
    public class ResetPasswordCode : EntityBase
    {
        public string Value { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
    }
}
