using FluentMigrator;

namespace Forum.Infrastructure.DataAccess.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_REFRESH_TOKENS, "Create table to save refresh tokens")]
    public class Version0000005 : VersionBase
    {
        public override void Up()
        {
            CreateTable("RefreshTokens")
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_RefreshToken_User_Id", "Users", "Id");
        }
    }
}
