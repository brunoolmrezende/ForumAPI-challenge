using FluentMigrator;

namespace Forum.Infrastructure.DataAccess.Migrations.Versions
{
    [Migration(DatabaseVersions.IMAGES_FOR_USERS, "Add collum on users to save profile image")]
    public class Version0000006 : VersionBase
    {
        public override void Up()
        {
            Alter.Table("Users")
                .AddColumn("ImageIdentifier").AsString(255).Nullable()
                .AddColumn("ImageUrl").AsString(255).Nullable();
        }
    }
}
