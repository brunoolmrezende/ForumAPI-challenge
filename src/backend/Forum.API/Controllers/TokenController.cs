using Forum.Application.UseCases.Token;
using Forum.Communication.Request;
using Forum.Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers
{
    public class TokenController : ForumControllerBase
    {
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(ResponseTokensJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken(
            [FromServices] IUseRefreshTokenUseCase useCase,
            [FromBody] RequestNewTokenJson request)
        {
            var response = await useCase.Execute(request);

            return Ok(response);
        }
    }
}
