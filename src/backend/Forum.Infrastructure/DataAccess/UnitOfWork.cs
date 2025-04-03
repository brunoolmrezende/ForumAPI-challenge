using Forum.Domain.Repository;

namespace Forum.Infrastructure.DataAccess
{
    public class UnitOfWork(ForumDbContext dbContext) : IUnitOfWork
    {
        private readonly ForumDbContext _dbContext = dbContext;

        public async Task Commit() => await _dbContext.SaveChangesAsync();
    }
}
