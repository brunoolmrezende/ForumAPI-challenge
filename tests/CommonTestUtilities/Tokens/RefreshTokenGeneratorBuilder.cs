using Forum.Domain.Security.RefreshToken;
using Forum.Infrastructure.Security.RefreshToken;

namespace CommonTestUtilities.Tokens
{
    public class RefreshTokenGeneratorBuilder
    {
        public static IRefreshTokenGenerator Build()
        {
            return new RefreshTokenGenerator();
        }
    }
}
