using Forum.Domain.Entities;
using Forum.Domain.Repository.Topic;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.DataAccess.Repositories
{
    public class TopicRepository(ForumDbContext dbContext) : ITopicWriteOnlyRepository, ITopicUpdateOnlyRepository
    {
        private readonly ForumDbContext _dbContext = dbContext;

        public async Task Add(Topic topic)
        {
            await _dbContext.Topics.AddAsync(topic);
        }

        public async Task<Topic?> GetById(long id, long loggedUserId)
        {
            return await _dbContext
                .Topics
                .FirstOrDefaultAsync(topic => topic.Id.Equals(id) && topic.UserId.Equals(loggedUserId) && topic.Active);
        }

        public void Update(Topic topic)
        {
            _dbContext.Topics.Update(topic);
        }
    }
}
