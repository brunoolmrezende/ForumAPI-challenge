using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Forum.API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [EnableRateLimiting("RateLimiterPolicy")]
    public class ForumControllerBase : ControllerBase
    {
    }
}
