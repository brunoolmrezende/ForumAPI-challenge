using Forum.Domain.Entities;
using Forum.Domain.Repository.ResetPasswordCode;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.DataAccess.Repositories
{
    public class ResetPasswordCodeRepository(ForumDbContext dbContext) : IResetPasswordCodeRepository
    {
        private readonly ForumDbContext _dbContext = dbContext;

        public async Task<ResetPasswordCode?> GetCode(string code, string email)
        {
            return await _dbContext
                .ResetPasswordCodes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Value.Equals(code) && c.UserEmail.Equals(email));
        }

        public async Task SaveNewCode(ResetPasswordCode resetPasswordCode)
        {
            var codes = _dbContext.ResetPasswordCodes.Where(code => code.UserEmail.Equals(resetPasswordCode.UserEmail));

            _dbContext.ResetPasswordCodes.RemoveRange(codes);

            await _dbContext.ResetPasswordCodes.AddAsync(resetPasswordCode);
        }
    }
}
