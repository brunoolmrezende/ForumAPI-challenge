using Forum.Domain.Entities;
using Forum.Domain.Repository.User;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.DataAccess.Repositories
{
    public class UserRepository(ForumDbContext dbContext) : IUserWriteOnlyRepository, IUserReadOnlyRepository
    {
        private readonly ForumDbContext _dbContext = dbContext;

        public async Task Add(Domain.Entities.User user)
        {
            await _dbContext.Users.AddAsync(user);
        }

        public async Task<bool> ExistActiveUserWithEmail(string email)
        {
            return await _dbContext.Users.AnyAsync(user => user.Email.Equals(email) && user.Active);
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email) && user.Active);
        }
    }
}
