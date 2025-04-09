using Forum.Application.UseCases.Forum;
using Forum.Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers
{
    public class ForumController : ForumControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponseTopicsJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll([FromServices] IGetAllTopicsUseCase useCase)
        {
            var response = await useCase.Execute();

            if (response.Topics.Count == 0)
            {
                return NoContent();
            }

            return Ok(response);
        }
    }
}
