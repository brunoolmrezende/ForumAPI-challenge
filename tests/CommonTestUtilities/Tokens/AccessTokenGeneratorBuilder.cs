using Forum.Domain.Security.AccessToken;
using Forum.Infrastructure.Security.AccessToken;

namespace CommonTestUtilities.Tokens
{
    public class AccessTokenGeneratorBuilder
    {
        public static IAccessTokenGenerator Build() => new AccessTokenGenerator(1000, "wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww");
    }
}
