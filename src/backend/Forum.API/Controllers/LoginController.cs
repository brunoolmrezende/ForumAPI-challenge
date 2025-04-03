using Forum.Application.UseCases.Login.DoLogin;
using Forum.Communication.Request;
using Forum.Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers
{
    public class LoginController : ForumControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DoLogin(
            [FromServices] IDoLoginUseCase useCase,
            [FromBody] RequestDoLoginJson request)
        {
            var response = await useCase.Execute(request);

            return Ok(response);
        }
    }
}
