using System.Net;
using System.Threading.RateLimiting;
using Forum.Communication.Response;
using Forum.Exceptions;
using Forum.Infrastructure.Extensions;
using Microsoft.AspNetCore.RateLimiting;

namespace Forum.API.RateLimit
{
    public class RateLimiterPolicy : IRateLimiterPolicy<string>
    {
        public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected => async (context, cancellationToken) =>
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            await context.HttpContext.Response.WriteAsJsonAsync(new ResponseErrorJson(ResourceMessagesException.TOO_MANY_REQUESTS), cancellationToken);
        };

        public RateLimitPartition<string> GetPartition(HttpContext httpContext)
        {
            var configuration = httpContext.RequestServices.GetRequiredService<IConfiguration>();

            bool isUnitTestEnvironment = configuration.IsUnitTestEnviroment();

            string clientIp = isUnitTestEnvironment
                ? "127.0.0.1" 
                : httpContext.Connection.RemoteIpAddress!.ToString();

            if (isUnitTestEnvironment)
            {
                return RateLimitPartition.GetNoLimiter<string>(clientIp);
            }
            else
            {
                return RateLimitPartition.GetFixedWindowLimiter(clientIp, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 10,
                    QueueLimit = 0,
                    Window = TimeSpan.FromMinutes(1),
                });
            }                     
        }
    }
}
