using Forum.Domain.Entities;
using Forum.Domain.Repository.Comment;

namespace Forum.Infrastructure.DataAccess.Repositories
{
    public class CommentRepository(ForumDbContext dbContext) : ICommentWriteOnlyRepository
    {
        private readonly ForumDbContext _dbContext = dbContext;

        public async Task Add(Comment comment)
        {
            await _dbContext.Comments.AddAsync(comment);
        }
    }
}
