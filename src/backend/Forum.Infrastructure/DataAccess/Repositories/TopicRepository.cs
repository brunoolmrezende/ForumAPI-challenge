using Forum.Domain.Entities;
using Forum.Domain.Repository.Topic;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.DataAccess.Repositories
{
    public class TopicRepository(ForumDbContext dbContext) : ITopicWriteOnlyRepository, ITopicUpdateOnlyRepository, ITopicReadOnlyRepository
    {
        private readonly ForumDbContext _dbContext = dbContext;

        public async Task Add(Topic topic)
        {
            await _dbContext.Topics.AddAsync(topic);
        }

        async Task<Topic?> ITopicUpdateOnlyRepository.GetById(long id, long loggedUserId)
        {
            return await _dbContext
                .Topics
                .FirstOrDefaultAsync(topic => topic.Id.Equals(id) && topic.UserId.Equals(loggedUserId) && topic.Active);
        }

        public void Update(Topic topic)
        {
            _dbContext.Topics.Update(topic);
        }

        async Task<Topic?> ITopicReadOnlyRepository.GetById(long id, long loggedUserId)
        {
            return await _dbContext
                .Topics
                .AsNoTracking()
                .FirstOrDefaultAsync(topic => topic.Id.Equals(id) && topic.UserId.Equals(loggedUserId) && topic.Active);
        }

        public async Task Delete(long id)
        {
            var topic = await _dbContext.Topics.FindAsync(id);

            _dbContext.Topics.Remove(topic!);
        }

        public async Task<bool> ExistsById(long topicId)
        {
            return await _dbContext
                .Topics
                .AsNoTracking()
                .AnyAsync(topic => topic.Id.Equals(topicId) && topic.Active);
        }
    }
}
