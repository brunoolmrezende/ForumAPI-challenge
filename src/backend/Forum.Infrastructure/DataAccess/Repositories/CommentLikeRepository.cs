using Forum.Domain.Entities;
using Forum.Domain.Repository.Like.CommentLike;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.DataAccess.Repositories
{
    public class CommentLikeRepository(ForumDbContext dbContext) : ICommentLikeWriteOnlyRepository, ICommentLikeUpdateOnlyRepository
    {
        private readonly ForumDbContext _dbContext = dbContext;

        public async Task Add(CommentLike commentLike)
        {
            await _dbContext.CommentLikes.AddAsync(commentLike);
        }

        public void Delete(CommentLike commentLike)
        {
            _dbContext.CommentLikes.Remove(commentLike);
        }

        public async Task<CommentLike?> GetById(long userId, long commentId)
        {
            return await _dbContext
                .CommentLikes
                .FirstOrDefaultAsync(commentLike => commentLike.CommentId.Equals(commentId) && commentLike.UserId.Equals(userId) && commentLike.Active);
        }
    }
}
