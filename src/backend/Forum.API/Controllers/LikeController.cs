using Forum.API.Attributes;
using Forum.Application.UseCases.Like;
using Forum.Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers
{
    [AuthenticatedUser]
    public class LikeController : ForumControllerBase
    {
        [HttpPost]
        [Route("{topicId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ToggleLike(
            [FromServices] IToggleLikeUseCase useCase,
            [FromRoute] long topicId)
        {
            await useCase.Execute(topicId);

            return NoContent();
        }
    }
}
