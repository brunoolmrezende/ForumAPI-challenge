using CommonTestUtilities.Entities;
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
        private Forum.Domain.Entities.Topic _topic = default!;
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

        public long GetTopicId() => _topic.Id;

        private void StartDatabase(ForumDbContext dbContext)
        {
            (_user, _password) = UserBuilder.Build();

            _topic = TopicBuilder.Build(_user);

            dbContext.Database.EnsureCreated();
            dbContext.Users.Add(_user);
            dbContext.Topics.Add(_topic);
            dbContext.SaveChanges();
        }
    }
}
