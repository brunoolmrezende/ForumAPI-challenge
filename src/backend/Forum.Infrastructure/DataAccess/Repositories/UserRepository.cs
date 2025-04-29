using Forum.Domain.Entities;
using Forum.Domain.Repository.User;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.DataAccess.Repositories
{
    public class UserRepository(ForumDbContext dbContext) : IUserWriteOnlyRepository, IUserReadOnlyRepository, IUserUpdateOnlyRepository, IUserDeleteOnlyRepository
    {
        private readonly ForumDbContext _dbContext = dbContext;

        public async Task Add(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }

        public async Task DeleteAccount(Guid userIdentifier)
        {
            var user = await _dbContext.Users.FirstAsync(user => user.UserIdentifier.Equals(userIdentifier));

            DeleteRelatedData(user.Id);

            _dbContext.Users.Remove(user);
        }

        public async Task<bool> ExistActiveUserWithEmail(string email)
        {
            return await _dbContext.Users.AnyAsync(user => user.Email.Equals(email) && user.Active);
        }

        public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier)
        {
            return await _dbContext.Users.AnyAsync(user => user.UserIdentifier.Equals(userIdentifier) && user.Active);
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email) && user.Active);
        }

        public async Task<User> GetById(long id)
        {
            return await _dbContext.Users.FirstAsync(user => user.Id.Equals(id)  && user.Active);
        }

        public async Task<User?> GetByIdentifier(Guid userIdentifier)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.UserIdentifier.Equals(userIdentifier));
        }

        public async Task<User> GetProfile(long id)
        {
            return await _dbContext
                .Users
                .AsNoTracking()
                .Include(user => user.Topics)
                .Include(user => user.Comments)
                .FirstAsync(user => user.Id.Equals(id) && user.Active);
        }

        public void Update(User user)
        {
            _dbContext.Users.Update(user);
        }

        private void DeleteRelatedData(long userId)
        {
            var topics = _dbContext.Topics.Where(topic => topic.UserId.Equals(userId));
            var comments = _dbContext.Comments.Where(comment => comment.UserId.Equals(userId));
            var likes = _dbContext.TopicLikes.Where(like => like.UserId.Equals(userId));
            var refreshToken = _dbContext.RefreshTokens.Where(token => token.UserId.Equals(userId));

            _dbContext.Topics.RemoveRange(topics);
            _dbContext.Comments.RemoveRange(comments);
            _dbContext.TopicLikes.RemoveRange(likes);
            _dbContext.RefreshTokens.RemoveRange(refreshToken);
        }
    }
}
