using FluentMigrator;

namespace Forum.Infrastructure.DataAccess.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_AUDIT_ENTRIES, "Add table to save audit entries")]
    public class Version0000008 : Migration
    {
        public override void Up()
        {
            Create.Table("AuditEntries")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("FieldName").AsString(255).NotNullable()
                .WithColumn("OldValue").AsString().Nullable()
                .WithColumn("NewValue").AsString().Nullable()
                .WithColumn("AuditId").AsInt64().NotNullable().ForeignKey("FK_AuditEntries_Audits", "Audits", "Id");
        }

        public override void Down()
        {
            Delete.Table("AuditEntries");
        }
    }
}
