using Microsoft.Extensions.Configuration;

namespace Forum.Infrastructure.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string ConnectionString(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("ConnectionSQLServer")!;
        }

        public static bool IsUnitTestEnviroment(this IConfiguration configuration) => configuration.GetValue<bool>("InMemoryTest");
    }
}
