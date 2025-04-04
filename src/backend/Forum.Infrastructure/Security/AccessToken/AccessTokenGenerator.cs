using Forum.Domain.Security.AccessToken;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Forum.Infrastructure.Security.AccessToken
{
    public class AccessTokenGenerator(uint expirationTimeMinutes, string signinKey) : IAccessTokenGenerator
    {
        private readonly uint _expirationTimeMinutes = expirationTimeMinutes;
        private readonly string _signinKey = signinKey;

        public string Generate(Guid userIdentifier)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Sid, userIdentifier.ToString()),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
                SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }

        private SymmetricSecurityKey SecurityKey()
        {
            var bytes = Encoding.UTF8.GetBytes(_signinKey);
            return new SymmetricSecurityKey(bytes);
        }
    }
}
