using Forum.Communication.Response;
using Forum.Domain.Repository.User;
using Forum.Domain.Security.AccessToken;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace Forum.API.Filters
{
    public class AuthenticatedUserFilter(
        IAccessTokenValidator accessTokenValidator,
        IUserReadOnlyRepository userReadOnlyRepository) : IAsyncAuthorizationFilter
    {
        private readonly IAccessTokenValidator _accessTokenValidator = accessTokenValidator;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository = userReadOnlyRepository;

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                var token = TokenOnRequest(context);

                var userIdentifier = _accessTokenValidator.ValidateAndGetUserIdentifier(token);

                var userExists = await _userReadOnlyRepository.ExistActiveUserWithIdentifier(userIdentifier);

                if (!userExists)
                {
                    throw new UnauthorizedException(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);
                }
            }
            catch (SecurityTokenExpiredException)
            {
                context.Result = new UnauthorizedObjectResult(new ResponseErrorJson("TokenIsExpired")
                {
                    TokenIsExpired = true
                });
            }
            catch (ForumException forumException)
            {
                context.Result = new ObjectResult(new ResponseErrorJson(forumException.GetErrorMessage()));
                context.HttpContext.Response.StatusCode = (int)forumException.GetHttpStatusCode();
            }
            catch
            {
                context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE));
            }
        }

        private static string TokenOnRequest(AuthorizationFilterContext context)
        {
            var authentication = context.HttpContext.Request.Headers.Authorization.ToString();

            if (string.IsNullOrWhiteSpace(authentication))
            {
                throw new UnauthorizedException(ResourceMessagesException.NO_TOKEN);
            }

            return authentication["Bearer ".Length..].Trim();
        }
    }
}
