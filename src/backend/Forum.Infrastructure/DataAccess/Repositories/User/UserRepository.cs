using Forum.Domain.Repository.User;

namespace Forum.Infrastructure.DataAccess.Repositories.User
{
    public class UserRepository(ForumDbContext dbContext) : IUserWriteOnlyRepository
    {
        private readonly ForumDbContext _dbContext = dbContext;

        public async Task Add(Domain.Entities.User user)
        {
            await _dbContext.Users.AddAsync(user);
        }
    }
}
