using CloudinaryDotNet;
using FluentMigrator.Runner;
using Forum.Domain.Repository;
using Forum.Domain.Repository.Comment;
using Forum.Domain.Repository.Like.CommentLike;
using Forum.Domain.Repository.Like.TopicLike;
using Forum.Domain.Repository.Token;
using Forum.Domain.Repository.Topic;
using Forum.Domain.Repository.User;
using Forum.Domain.Security.AccessToken;
using Forum.Domain.Security.Cryptography;
using Forum.Domain.Security.RefreshToken;
using Forum.Domain.Services;
using Forum.Infrastructure.DataAccess;
using Forum.Infrastructure.DataAccess.Repositories;
using Forum.Infrastructure.Extensions;
using Forum.Infrastructure.Security.AccessToken;
using Forum.Infrastructure.Security.Cryptography;
using Forum.Infrastructure.Security.RefreshToken;
using Forum.Infrastructure.Services.DeleteUserQueue;
using Forum.Infrastructure.Services.LoggedUser;
using Forum.Infrastructure.Services.Photo;
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
            AddQueue(services, configuration);

            if (configuration.IsUnitTestEnviroment())
            {
                return;
            }

            var databaseType = configuration.DatabaseType();

            if (databaseType == Domain.Enums.DatabaseType.SqlServer)
            {
                AddDbContext_SqlServer(services, configuration);
                AddFluentMigrator_SqlServer(services, configuration);
            }
            else
            {
                AddDbContext_PostgreSql(services, configuration);
                AddFluentMigrator_PostgreSql(services, configuration);
            }

            AddCloudinary(services, configuration);
        }

        private static void AddDbContext_SqlServer(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();

            services.AddDbContext<ForumDbContext>(dbContextOptions =>
            {
                dbContextOptions.UseSqlServer(connectionString);
            });
        }

        private static void AddDbContext_PostgreSql(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();

            services.AddDbContext<ForumDbContext>(dbContextOptions =>
            {
                dbContextOptions.UseNpgsql(connectionString);
            });
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();
            services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
            services.AddScoped<IUserDeleteOnlyRepository, UserRepository>();

            services.AddScoped<ITopicWriteOnlyRepository, TopicRepository>();
            services.AddScoped<ITopicUpdateOnlyRepository, TopicRepository>();
            services.AddScoped<ITopicReadOnlyRepository, TopicRepository>();

            services.AddScoped<ICommentWriteOnlyRepository, CommentRepository>();
            services.AddScoped<ICommentUpdateOnlyRepository, CommentRepository>();

            services.AddScoped<ITopicLikeUpdateOnlyRepository, TopicLikeRepository>();
            services.AddScoped<ITopicLikeWriteOnlyRepository, TopicLikeRepository>();

            services.AddScoped<ICommentLikeUpdateOnlyRepository, CommentLikeRepository>();
            services.AddScoped<ICommentLikeWriteOnlyRepository, CommentLikeRepository>();

            services.AddScoped<ITokenRepository, TokenRepository>();

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

            services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
        }

        private static void AddLoggedUser(IServiceCollection services)
        {
            services.AddScoped<ILoggedUser, LoggedUser>();
        }

        private static void AddCloudinary(IServiceCollection services, IConfiguration configuration)
        {
            var cloudName = configuration.GetValue<string>("Settings:Cloudinary:CloudName");
            var apiKey = configuration.GetValue<string>("Settings:Cloudinary:ApiKey");
            var apiSecret = configuration.GetValue<string>("Settings:Cloudinary:ApiSecret");

            var account = new Account(cloudName, apiKey, apiSecret);

            var cloudinary = new Cloudinary(account) { Api = { Secure = true } };

            services.AddSingleton(cloudinary);

            services.AddScoped<IPhotoService>(options => new PhotoService(cloudinary));
        }

        private static void AddQueue(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetValue<string>("Settings:RabbitMQ:Connection")!;
            var queueName = configuration.GetValue<string>("Settings:RabbitMQ:QueueName")!;

            services.AddScoped<IDeleteUserQueue>(options => new DeleteUserQueue(connection, queueName));
        }

        private static void AddFluentMigrator_SqlServer(IServiceCollection services, IConfiguration configuration)
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

        private static void AddFluentMigrator_PostgreSql(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();

            services.AddFluentMigratorCore().ConfigureRunner(options =>
            {
                options
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("Forum.Infrastructure")).For.All();
            });
        }
    }
}
