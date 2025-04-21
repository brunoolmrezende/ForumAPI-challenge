using Microsoft.AspNetCore.Http;

namespace Forum.Communication.Request
{
    public class RequestRegisterUserFormData : RequestRegisterUserJson
    {
        public IFormFile? Image { get; set; }
    }
}
