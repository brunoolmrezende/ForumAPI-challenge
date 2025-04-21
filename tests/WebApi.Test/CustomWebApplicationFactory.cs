using CommonTestUtilities.Entities;
using CommonTestUtilities.Services.Photo;
using Forum.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private Forum.Domain.Entities.User _user = default!;
        private Forum.Domain.Entities.User _userWithoutPhoto = default!;
        private Forum.Domain.Entities.Topic _topic = default!;
        private Forum.Domain.Entities.Comment _comment = default!;
        private Forum.Domain.Entities.TopicLike _topicLike = default!;
        private Forum.Domain.Entities.RefreshToken _refreshToken = default!;
        private string _password = string.Empty;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        { 
            builder.UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ForumDbContext>));

                    if (descriptor is not null)
                    {
                        services.Remove(descriptor);
                    }

                    var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    var photoService = new PhotoServiceBuilder().Build();
                    services.AddScoped(option => photoService);

                    services.AddDbContext<ForumDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                        options.UseInternalServiceProvider(provider);
                    });

                    using var scope = services.BuildServiceProvider().CreateScope();

                    var database = scope.ServiceProvider.GetRequiredService<ForumDbContext>();
                    database.Database.EnsureDeleted();

                    StartDatabase(database);
                });
        }

        public string GetName() => _user.Name;
        public string GetEmail() => _user.Email;
        public string GetPassword() => _password;
        public Guid GetIdentifier() => _user.UserIdentifier;
        public Guid GetIdentifierFromUserWithoutPhoto() => _userWithoutPhoto.UserIdentifier;

        public Forum.Domain.Entities.Topic GetTopic() => _topic;
        public long GetTopicId() => _topic.Id;
        public string GetTopicTitle() => _topic.Title;
        public string GetTopicContent() => _topic.Content;

        public long GetCommentId() => _comment.Id;

        public string GetRefreshToken() => _refreshToken.Value;

        private void StartDatabase(ForumDbContext dbContext)
        {
            (_user, _password) = UserBuilder.Build();

            (_userWithoutPhoto, _) = UserBuilder.Build();
            _userWithoutPhoto.ImageIdentifier = null;
            _userWithoutPhoto.ImageUrl = null;
            _userWithoutPhoto.Id = 2;

            _topic = TopicBuilder.Build(_user);

            _comment = CommentBuilder.Build(_user, _topic.Id);

            _topicLike = TopicLikeBuilder.Build(_topic, _user);

            _refreshToken = RefreshTokenBuilder.Build(_user);

            dbContext.Database.EnsureCreated();
            dbContext.Users.Add(_user);
            dbContext.Users.Add(_userWithoutPhoto);
            dbContext.Topics.Add(_topic);
            dbContext.Comments.Add(_comment);
            dbContext.TopicLikes.Add(_topicLike);
            dbContext.RefreshTokens.Add(_refreshToken);
            dbContext.SaveChanges();
        }
    }
}
