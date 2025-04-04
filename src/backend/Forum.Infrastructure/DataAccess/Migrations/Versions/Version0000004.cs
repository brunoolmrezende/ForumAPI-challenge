using FluentMigrator;

namespace Forum.Infrastructure.DataAccess.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_TOPIC_LIKES, "Create table for topic likes")]
    public class Version0000004 : VersionBase
    {
        public override void Up()
        {
            CreateTable("TopicLikes")
                .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_TopicLike_User_Id", "Users", "Id")
                .WithColumn("TopicId").AsInt64().NotNullable().ForeignKey("FK_TopicLike_Topic_Id", "Topics", "Id");
        }
    }
}
