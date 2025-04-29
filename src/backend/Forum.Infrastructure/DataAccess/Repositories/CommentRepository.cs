using Forum.Domain.Entities;
using Forum.Domain.Repository.Comment;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.DataAccess.Repositories
{
    public class CommentRepository(ForumDbContext dbContext) : ICommentWriteOnlyRepository, ICommentUpdateOnlyRepository
    {
        private readonly ForumDbContext _dbContext = dbContext;

        public async Task Add(Comment comment)
        {
            await _dbContext.Comments.AddAsync(comment);
        }

        public void Delete(Comment comment)
        {
            _dbContext.Comments.Remove(comment);
        }

        public async Task<Comment?> GetById(long id, long loggedUserId)
        {
            return await _dbContext
                .Comments
                .FirstOrDefaultAsync(comment => comment.Id.Equals(id) && comment.UserId.Equals(loggedUserId) && comment.Active);
        }

        public void Update(Comment comment)
        {
            _dbContext.Comments.Update(comment);
        }
    }
}
