using Forum.Application.UseCases.Auth.ForgotPassword;
using Forum.Application.UseCases.Auth.Login.DoLogin;
using Forum.Application.UseCases.Auth.ResetPassword;
using Forum.Communication.Request;
using Forum.Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers
{
    public class AuthController : ForumControllerBase
    {
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DoLogin(
            [FromServices] IDoLoginUseCase useCase,
            [FromBody] RequestDoLoginJson request)
        {
            var response = await useCase.Execute(request);

            return Ok(response);
        }

        [HttpPost]
        [Route("forgot-password")]
        [ProducesResponseType(typeof(ResponseMessageJson), StatusCodes.Status202Accepted)]
        public async Task<IActionResult> ForgotPassword(
            [FromServices] IForgotPasswordUseCase useCase,
            [FromBody] RequestForgotPasswordJson request)
        {
            var response = await useCase.Execute(request);

            return Accepted(response);
        }

        [HttpPost]
        [Route("reset-password")]
        [ProducesResponseType(typeof(ResponseMessageJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword(
            [FromServices] IResetPasswordUseCase useCase,
            [FromBody] RequestResetPasswordJson request)
        {
            var response = await useCase.Execute(request);
            return Ok(response);
        }
    }
}
