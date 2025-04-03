using FluentMigrator.Runner;
using Forum.Domain.Repository;
using Forum.Domain.Repository.User;
using Forum.Domain.Security.Cryptography;
using Forum.Infrastructure.Cryptography;
using Forum.Infrastructure.DataAccess;
using Forum.Infrastructure.DataAccess.Repositories.User;
using Forum.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Forum.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext(services, configuration);
            AddFluentMigrator(services, configuration);
            AddRepositories(services);
            AddPasswordEncrypter(services);
        }

        private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();

            services.AddDbContext<ForumDbContext>(dbContextOptions =>
            {
                dbContextOptions.UseSqlServer(connectionString);
            });
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static void AddPasswordEncrypter(this IServiceCollection services)
        {
            services.AddScoped<IPasswordEncryption, BCryptNet>();
        }

        private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();

            services.AddFluentMigratorCore().ConfigureRunner(options =>
            {
                options
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("Forum.Infrastructure")).For.All();
            });
        }
    }
}
