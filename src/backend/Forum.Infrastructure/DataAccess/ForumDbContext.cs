using Forum.Domain.Entities;
using Forum.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Forum.Infrastructure.DataAccess
{
    public class ForumDbContext(DbContextOptions options) : DbContext(options)
    {
        private readonly List<(Audit audit, EntityEntry entityEntry)> _pendingAudits = [];

        public DbSet<User> Users { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<TopicLike> TopicLikes { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<AuditEntry> AuditEntries { get; set; }
        public DbSet<ResetPasswordCode> ResetPasswordCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ForumDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            TrackAuditChanges();

            var result = await base.SaveChangesAsync(cancellationToken);

            if (_pendingAudits.Count > 0)
            {
                await SaveAuditChangesAsync(cancellationToken);
                _pendingAudits.Clear();
            }

            return result;
        }

        private void TrackAuditChanges()
        {
            var entities = ChangeTracker
                .Entries()
                .Where(entity => entity.State is not EntityState.Unchanged).ToList();

            foreach (var entity in entities)
            {
                var entityBase = (EntityBase)entity.Entity;

                var audit = new Audit
                {
                    Operation = entity.State.OperationType(),
                    TableName = entity.Metadata?.GetTableName() ?? entity.GetType().Name,
                    RecordId = entityBase.Id,
                };

                foreach (var property in entity.Properties)
                {
                    if (property.Metadata.Name.Equals("Id") || property.Metadata.Name.Equals("CreatedOn"))
                        continue;

                    var auditEntry = new AuditEntry
                    {
                        FieldName = property.Metadata.Name,
                        OldValue = entity.State is EntityState.Added ? null : property.OriginalValue?.ToString(),
                        NewValue = entity.State is EntityState.Deleted ? null : property.CurrentValue?.ToString(),
                        AuditId = audit.Id
                    };

                    if (
                        (auditEntry.OldValue is not null && Equals(auditEntry.NewValue, auditEntry.OldValue))
                        ||
                        (auditEntry.OldValue is null && auditEntry.NewValue is null)
                        )
                        continue;

                    audit.Changes.Add(auditEntry);
                }

                if (audit.Changes.Count > 0)
                {
                    _pendingAudits.Add((audit, entity));
                }
            }
        }

        private async Task SaveAuditChangesAsync(CancellationToken cancellationToken)
        {
            foreach (var (audit, entityEntry) in _pendingAudits)
            {
                audit.RecordId = ((EntityBase)entityEntry.Entity).Id;

                await Audits.AddAsync(audit, cancellationToken);
            }

            await base.SaveChangesAsync(cancellationToken);
        }
    }
}
