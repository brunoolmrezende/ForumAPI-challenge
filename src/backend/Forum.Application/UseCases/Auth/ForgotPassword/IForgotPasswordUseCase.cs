using Forum.Communication.Request;
using Forum.Communication.Response;

namespace Forum.Application.UseCases.Auth.ForgotPassword
{
    public interface IForgotPasswordUseCase
    {
        Task<ResponseMessageJson> Execute(RequestForgotPasswordJson request);
    }
}
