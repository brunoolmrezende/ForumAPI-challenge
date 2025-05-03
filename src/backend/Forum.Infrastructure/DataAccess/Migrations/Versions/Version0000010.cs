using FluentMigrator;

namespace Forum.Infrastructure.DataAccess.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_RESET_PASSWORD_CODES, "Create table to save reset password codes")]
    public class Version0000010 : VersionBase
    {
        public override void Up()
        {
            CreateTable("ResetPasswordCodes")
                .WithColumn("Value").AsString().NotNullable()
                .WithColumn("UserEmail").AsString().NotNullable();
        }
    }
}
