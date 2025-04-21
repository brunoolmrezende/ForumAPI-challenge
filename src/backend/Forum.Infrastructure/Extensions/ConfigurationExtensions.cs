using Forum.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace Forum.Infrastructure.Extensions
{
    public static class ConfigurationExtensions
    {

        public static DatabaseType DatabaseType(this IConfiguration configurarion)
        {
            var databaseType = configurarion.GetConnectionString("DatabaseType");

            return (DatabaseType)Enum.Parse(typeof(DatabaseType), databaseType!);
        }

        public static string ConnectionString(this IConfiguration configuration)
        {
            var databaseType = configuration.DatabaseType();

            if (databaseType == Domain.Enums.DatabaseType.PostgreSQL)
            {
                return configuration.GetConnectionString("ConnectionPostgreSQL")!;
            }
            else
            {
                return configuration.GetConnectionString("ConnectionSQLServer")!;
            }
        }

        public static bool IsUnitTestEnviroment(this IConfiguration configuration) => configuration.GetValue<bool>("InMemoryTest");
    }
}
