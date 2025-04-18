using Forum.Domain.Entities;
using Forum.Domain.Repository.Token;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.DataAccess.Repositories
{
    public class TokenRepository(ForumDbContext dbContext) : ITokenRepository
    {
        private readonly ForumDbContext _dbContext = dbContext;

        public async Task<RefreshToken?> GetToken(string refreshToken)
        {
            return await _dbContext
                .RefreshTokens
                .AsNoTracking()
                .Include(token => token.User)
                .FirstOrDefaultAsync(token => token.Value == refreshToken);
        }

        public async Task SaveNewRefreshToken(RefreshToken refreshToken)
        {
            var tokens = _dbContext.RefreshTokens.Where(token => token.UserId.Equals(refreshToken.UserId));

            _dbContext.RefreshTokens.RemoveRange(tokens);

            await _dbContext.RefreshTokens.AddAsync(refreshToken);
        }
    }
}
