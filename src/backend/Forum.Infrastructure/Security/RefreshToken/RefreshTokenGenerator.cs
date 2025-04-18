using Forum.Domain.Security.RefreshToken;

namespace Forum.Infrastructure.Security.RefreshToken
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        public string Generate()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}
