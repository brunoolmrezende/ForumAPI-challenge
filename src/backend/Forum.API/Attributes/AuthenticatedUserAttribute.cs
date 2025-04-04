using Forum.API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Attributes
{
    public class AuthenticatedUserAttribute : TypeFilterAttribute
    {
        public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter))
        {
        }
    }
}
