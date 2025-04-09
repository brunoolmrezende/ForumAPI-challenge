using Forum.API.Attributes;
using Forum.Application.UseCases.Topic.Delete;
using Forum.Application.UseCases.Topic.Filter;
using Forum.Application.UseCases.Topic.GetById;
using Forum.Application.UseCases.Topic.Register;
using Forum.Application.UseCases.Topic.Update;
using Forum.Communication.Request;
using Forum.Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers
{
    public class TopicController : ForumControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredTopicJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [AuthenticatedUser]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterTopicUseCase useCase,
            [FromBody] RequestTopicJson request)
        {
            var response = await useCase.Execute(request);

            return Created(string.Empty, response);
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [AuthenticatedUser]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateTopicUseCase useCase,
            [FromBody] RequestTopicJson request,
            [FromRoute] long id)
        {
            await useCase.Execute(request, id);

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        [AuthenticatedUser]
        public async Task<IActionResult> Delete(
            [FromServices] IDeleteTopicUseCase useCase,
            [FromRoute] long id)
        {
            await useCase.Execute(id);

            return NoContent();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseTopicDetailsJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromServices] IGetTopicByIdUseCase useCase, 
            [FromRoute] long id)
        {
            var response = await useCase.Execute(id);

            return Ok(response);
        }

        [HttpPost("filter")]
        [ProducesResponseType(typeof(ResponseTopicsJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Filter(
            [FromServices] IFilterTopicUseCase useCase,
            [FromBody] RequestFilterTopicJson request)
        {
            var response = await useCase.Execute(request);

            if (response.Topics.Count == 0)
            {
                return NoContent();
            }

            return Ok(response);
        }
    }
}
