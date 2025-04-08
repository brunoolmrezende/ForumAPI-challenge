using Forum.Domain.Entities;
using Forum.Domain.Repository.Like.TopicLike;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.DataAccess.Repositories
{
    public class TopicLikeRepository(ForumDbContext dbContext) : ITopicLikeUpdateOnlyRepository, ITopicLikeWriteOnlyRepository
    {
        private readonly ForumDbContext _dbContext = dbContext;

        public async Task Add(TopicLike topicLike)
        {
            await _dbContext.TopicLikes.AddAsync(topicLike);
        }

        public void Delete(TopicLike topicLike)
        {
            _dbContext.TopicLikes.Remove(topicLike);
        }

        public async Task<TopicLike?> GetById(long userId, long topicId)
        {
            return await _dbContext
                .TopicLikes
                .FirstOrDefaultAsync(topicLike => topicLike.TopicId.Equals(topicId) && topicLike.UserId.Equals(userId) && topicLike.Active);
        }
    }
}
