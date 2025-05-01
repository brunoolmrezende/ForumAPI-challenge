using Forum.Domain.Dtos;
using Forum.Domain.Entities;
using Forum.Domain.Repository.Topic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

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

        public async Task<Topic?> GetTopicDetails(long id)
        {
            return await GetFullTopic()
                .AsNoTracking()
                .FirstOrDefaultAsync(topic => topic.Id.Equals(id) && topic.Active);
        }

        public async Task<List<Topic>> GetAllTopics()
        {
            return await GetFullTopic()
                .AsNoTracking()
                .OrderByDescending(topic => topic.CreatedOn)
                .ToListAsync();
        }

        public async Task<List<Topic>> Filter(FilterTopicDto filters)
        {
            var query = GetFullTopic().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filters.TopicTitle))
            {
                query = query.Where(topic => topic.Title.Contains(filters.TopicTitle));
            }

            if (!string.IsNullOrWhiteSpace(filters.TopicContent))
            {
                query = query.Where(topic => topic.Content.Contains(filters.TopicContent));
            }

            query = filters.OrderBy?.ToLower() switch
            {
                "likes" => query.OrderByDescending(topic => topic.Likes.Count),
                "comments" => query.OrderByDescending(topic => topic.Comments.Count),
                "title" => query.OrderBy(topic => topic.Title),
                _ => query.OrderByDescending(topic => topic.CreatedOn)
            };

            return await query.ToListAsync();
        }

        private IIncludableQueryable<Topic, List<CommentLike>> GetFullTopic()
        {
            return _dbContext
                .Topics
                .Where(topic => topic.Active)
                .Include(topic => topic.User)
                .Include(topic => topic.Likes)
                .Include(topic => topic.Comments)
                    .ThenInclude(comment => comment.User)
                .Include(topic => topic.Comments)
                    .ThenInclude(comment => comment.Likes);
        }
    }
}
