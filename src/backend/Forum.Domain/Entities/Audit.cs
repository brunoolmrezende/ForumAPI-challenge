namespace Forum.Domain.Entities
{
    public class Audit
    {
        public long Id { get; set; }
        public string Operation { get; set; } = string.Empty;
        public string TableName { get; set; } = string.Empty;
        public DateTime ChangeDate { get; set; } = DateTime.UtcNow;
        public long RecordId { get; set; }
        public IList<AuditEntry> Changes { get; set; } = [];
    }
}
