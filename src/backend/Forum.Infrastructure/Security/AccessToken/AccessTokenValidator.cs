using Forum.Domain.Security.AccessToken;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Forum.Infrastructure.Security.AccessToken
{
    public class AccessTokenValidator(string singinKey) : JwtTokenHandler, IAccessTokenValidator
    {
        private readonly string _signinKey = singinKey;

        public Guid ValidateAndGetUserIdentifier(string token)
        {
            var validationParameter = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = SecurityKey(_signinKey),
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, validationParameter, out var _);

            var userIdentifier = principal.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

            return Guid.Parse(userIdentifier);
        }
    }
}
