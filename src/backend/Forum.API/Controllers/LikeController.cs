using Forum.API.Attributes;
using Forum.Application.UseCases.Like.Comment;
using Forum.Application.UseCases.Like.Topic;
using Forum.Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers
{
    [AuthenticatedUser]
    public class LikeController : ForumControllerBase
    {
        [HttpPost]
        [Route("topic/{topicId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ToggleLike(
            [FromServices] IToggleTopicLikeUseCase useCase,
            [FromRoute] long topicId)
        {
            await useCase.Execute(topicId);

            return NoContent();
        }

        [HttpPost]
        [Route("comment/{commentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ToggleCommentLike(
            [FromServices] IToggleCommentLikeUseCase useCase,
            [FromRoute] long commentId)
        {
            await useCase.Execute(commentId);

            return NoContent();
        }
    }
}
