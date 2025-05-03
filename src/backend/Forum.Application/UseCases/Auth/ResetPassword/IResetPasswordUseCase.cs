using Forum.Communication.Request;
using Forum.Communication.Response;

namespace Forum.Application.UseCases.Auth.ResetPassword
{
    public interface IResetPasswordUseCase
    {
        Task<ResponseMessageJson> Execute(RequestResetPasswordJson request);
    }
}
