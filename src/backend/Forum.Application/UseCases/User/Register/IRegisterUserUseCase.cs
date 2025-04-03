using Forum.Communication.Request;
using Forum.Communication.Response;

namespace Forum.Application.UseCases.User.Register
{
    public interface IRegisterUserUseCase
    {
        Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request);
    }
}
