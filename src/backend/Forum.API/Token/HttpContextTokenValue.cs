using Forum.Domain.Security.AccessToken;

namespace Forum.API.Token
{
    public class HttpContextTokenValue(IHttpContextAccessor httpContextAccessor) : ITokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string Value()
        {
            var authentication = _httpContextAccessor.HttpContext!.Response.Headers.Authorization.ToString();

            return authentication["Bearer ".Length..].Trim();
        }
    }
}
