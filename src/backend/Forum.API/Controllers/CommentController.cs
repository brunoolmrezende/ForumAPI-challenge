using Forum.API.Attributes;
using Forum.Application.UseCases.Comment.Register;
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
    }
}
