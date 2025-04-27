namespace Forum.Domain.Entities
{
    public class AuditEntry
    {
        public long Id { get; set; }
        public string FieldName { get; set; } = string.Empty;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public long AuditId { get; set; }
    }
}
