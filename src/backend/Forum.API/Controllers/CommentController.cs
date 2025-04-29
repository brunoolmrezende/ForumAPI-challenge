using Forum.API.Attributes;
using Forum.Application.UseCases.Comment.Delete;
using Forum.Application.UseCases.Comment.Register;
using Forum.Application.UseCases.Comment.Update;
using Forum.Communication.Request;
using Forum.Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers
{
    [AuthenticatedUser]
    public class CommentController : ForumControllerBase
    {
        [HttpPost]
        [Route("{topicId}")]
        [ProducesResponseType(typeof(ResponseRegisteredCommentJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create(
            [FromServices] IRegisterCommentUseCase useCase,
            [FromRoute] long topicId,
            [FromBody] RequestCommentJson request)
        {
            var response = await useCase.Execute(topicId, request);

            return Created(string.Empty, response);
        }

        [HttpPut]
        [Route("{commentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateCommentUseCase useCase,
            [FromRoute]long topicId,
            [FromRoute] long commentId,
            [FromBody] RequestCommentJson request)
        {
            await useCase.Execute(commentId, request);

            return NoContent();
        }

        [HttpDelete]
        [Route("{topicId}/{commentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromServices] IDeleteCommentUseCase useCase,
            [FromRoute] long topicId,
            [FromRoute] long commentId)
        {
            await useCase.Execute(topicId, commentId);

            return NoContent();
        }
    }
}
