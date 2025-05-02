using Forum.Communication.Request;
using Forum.Communication.Response;

namespace Forum.Application.UseCases.Auth.Login.DoLogin
{
    public interface IDoLoginUseCase
    {
        Task<ResponseRegisteredUserJson> Execute(RequestDoLoginJson request);
    }
}
