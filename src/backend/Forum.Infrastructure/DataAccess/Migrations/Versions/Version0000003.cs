using FluentMigrator;
using System.Data;

namespace Forum.Infrastructure.DataAccess.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_COMMENTS, "Create table to save the user's comments")]
    public class Version0000003 : VersionBase
    {
        public override void Up()
        {
            CreateTable("Comments")
                .WithColumn("Content").AsString(2000).NotNullable()
                .WithColumn("TopicId").AsInt64().NotNullable().ForeignKey("FK_Comment_Topic_Id", "Topics", "Id").OnDelete(Rule.Cascade)
                .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Comment_User_Id", "Users", "Id");
        }
    }
}
