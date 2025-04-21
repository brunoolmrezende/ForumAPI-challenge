using Forum.API.Attributes;
using Forum.Application.UseCases.User.Change_Password;
using Forum.Application.UseCases.User.Delete_Image;
using Forum.Application.UseCases.User.Image;
using Forum.Application.UseCases.User.Profile;
using Forum.Application.UseCases.User.Register;
using Forum.Application.UseCases.User.Update;
using Forum.Communication.Request;
using Forum.Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers
{
    public class UserController : ForumControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterUserUseCase useCase,
            [FromForm] RequestRegisterUserFormData request)
        {
            var response = await useCase.Execute(request);

            return Created(string.Empty, response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [AuthenticatedUser]
         public async Task<IActionResult> Update(
             [FromServices] IUpdateUserUseCase useCase,
             [FromBody] RequestUpdateUserJson request)
         {
            await useCase.Execute(request);

            return NoContent();
         }

        [HttpPut("change-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [AuthenticatedUser]
        public async Task<IActionResult> ChangePassword(
            [FromServices] IChangePasswordUseCase useCase,
            [FromBody] RequestChangePasswordJson request)
        {
            await useCase.Execute(request);

            return NoContent();
        }

        [HttpPut("update-photo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [AuthenticatedUser]
        public async Task<IActionResult> UpdateImage(
            [FromServices] IAddUpdateImageUseCase useCase,
            IFormFile file)
        {
            await useCase.Execute(file);

            return NoContent();
        }

        [HttpDelete("delete-photo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        [AuthenticatedUser]
        public async Task<IActionResult> DeleteImage(
            [FromServices] IDeleteImageUseCase useCase)
        {
            await useCase.Execute();

            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
        [AuthenticatedUser]
        public async Task<IActionResult> GetUserProfile(
            [FromServices] IGetUserProfileUseCase useCase)
        {
            var response = await useCase.Execute();

            return Ok(response);
        }

    }
}
