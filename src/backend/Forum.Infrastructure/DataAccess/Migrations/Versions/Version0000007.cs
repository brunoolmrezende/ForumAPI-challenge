using FluentMigrator;

namespace Forum.Infrastructure.DataAccess.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_AUDITS, "Add table to save audit events")]
    public class Version0000007 : Migration
    {
        public override void Up()
        {
            Create.Table("Audits")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Operation").AsString(255).NotNullable()
                .WithColumn("TableName").AsString(255).NotNullable()
                .WithColumn("ChangeDate").AsDateTime().NotNullable()
                .WithColumn("RecordId").AsInt64().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Audits");
        }
    }
}
