using FluentMigrator;
using System.Data;

namespace Forum.Infrastructure.DataAccess.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_COMMENT_LIKES, "Create table for comment likes")]
    public class Version0000009 : VersionBase
    {
        public override void Up()
        {
            CreateTable("CommentLikes")
                .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_CommentLike_User_Id", "Users", "Id")
                .WithColumn("CommentId").AsInt64().NotNullable().ForeignKey("FK_CommentLike_Comment_Id", "Comments", "Id").OnDelete(Rule.Cascade);

            Create.UniqueConstraint("UQ_CommentLike_Comment_User")
                .OnTable("CommentLikes")
                .Columns("CommentId", "UserId");
        }
    }
}
