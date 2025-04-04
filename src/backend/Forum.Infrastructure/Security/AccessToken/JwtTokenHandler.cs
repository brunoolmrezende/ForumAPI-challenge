using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Forum.Infrastructure.Security.AccessToken
{
    public abstract class JwtTokenHandler
    {
        protected static SymmetricSecurityKey SecurityKey(string signinKey)
        {
            var bytes = Encoding.UTF8.GetBytes(signinKey);
            return new SymmetricSecurityKey(bytes);
        }
    }
}
