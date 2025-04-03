using Forum.Communication.Response;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Forum.API.Filters
{
    public class ExceptionFilters : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ForumException forumException)
            {
                HandleProjectException(context, forumException);
            }
            else
            {
                ThrowUnknownException(context);
            }
        }

        private static void HandleProjectException(ExceptionContext context, ForumException forumException)
        {
            context.HttpContext.Response.StatusCode = (int)forumException.GetHttpStatusCode();
            context.Result = new ObjectResult(new ResponseErrorJson(forumException.GetErrorMessage()));
        }

        private static void ThrowUnknownException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOWN_ERROR));
        }
    }
}
