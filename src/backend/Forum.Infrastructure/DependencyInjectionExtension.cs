using FluentMigrator.Runner;
using Forum.Domain.Repository;
using Forum.Domain.Repository.Topic;
using Forum.Domain.Repository.User;
using Forum.Domain.Security.AccessToken;
using Forum.Domain.Security.Cryptography;
using Forum.Domain.Services;
using Forum.Infrastructure.DataAccess;
using Forum.Infrastructure.DataAccess.Repositories;
using Forum.Infrastructure.Extensions;
using Forum.Infrastructure.Security.AccessToken;
using Forum.Infrastructure.Security.Cryptography;
using Forum.Infrastructure.Services.LoggedUser;
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
            AddRepositories(services);
            AddPasswordEncrypter(services);
            AddTokens(services, configuration);
            AddLoggedUser(services);

            if (configuration.IsUnitTestEnviroment())
            {
                return;
            }

            AddDbContext(services, configuration);
            AddFluentMigrator(services, configuration);
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();

            services.AddDbContext<ForumDbContext>(dbContextOptions =>
            {
                dbContextOptions.UseSqlServer(connectionString);
            });
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();

            services.AddScoped<ITopicWriteOnlyRepository, TopicRepository>();
            services.AddScoped<ITopicUpdateOnlyRepository, TopicRepository>();
            services.AddScoped<ITopicReadOnlyRepository, TopicRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static void AddPasswordEncrypter(IServiceCollection services)
        {
            services.AddScoped<IPasswordEncryption, BCryptNet>();
        }

        private static void AddTokens(IServiceCollection services, IConfiguration configuration)
        {
            var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
            var signinKey = configuration.GetValue<string>("Settings:Jwt:SigninKey")!;

            services.AddScoped<IAccessTokenGenerator>(options => new AccessTokenGenerator(expirationTimeMinutes, signinKey));
            services.AddScoped<IAccessTokenValidator>(options => new AccessTokenValidator(signinKey));
        }

        private static void AddLoggedUser(IServiceCollection services)
        {
            services.AddScoped<ILoggedUser, LoggedUser>();
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
