using FluentMigrator;

namespace Forum.Infrastructure.DataAccess.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_TOPICS, "Create table to save the user's topics")]
    public class Version0000002 : VersionBase
    {
        public override void Up()
        {
            CreateTable("Topics")
                .WithColumn("Title").AsString(255).NotNullable()
                .WithColumn("Content").AsString(2000).NotNullable()
                .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Topic_User_Id", "Users", "Id");
        }
    }
}
