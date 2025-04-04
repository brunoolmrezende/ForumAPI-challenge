using Forum.Domain.Entities;
using Forum.Domain.Security.AccessToken;
using Forum.Domain.Services;
using Forum.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Forum.Infrastructure.Services.LoggedUser
{
    public class LoggedUser(ForumDbContext dbContext, ITokenProvider tokenProvider) : ILoggedUser
    {
        private readonly ForumDbContext _dbContext = dbContext;
        private readonly ITokenProvider _tokenProvider = tokenProvider;

        public async Task<User> User()
        {
            var token = _tokenProvider.Value();

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

            var userIdentifier = Guid.Parse(identifier);

            return await _dbContext.Users.AsNoTracking().FirstAsync(user => user.UserIdentifier.Equals(userIdentifier) && user.Active);
        }
    }
}
