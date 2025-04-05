using Forum.Domain.Entities;
using Forum.Domain.Repository.Topic;

namespace Forum.Infrastructure.DataAccess.Repositories
{
    public class TopicRepository(ForumDbContext dbContext) : ITopicWriteOnlyRepository
    {
        private readonly ForumDbContext _dbContext = dbContext;

        public async Task Add(Topic topic)
        {
            await _dbContext.Topics.AddAsync(topic);
        }
    }
}
