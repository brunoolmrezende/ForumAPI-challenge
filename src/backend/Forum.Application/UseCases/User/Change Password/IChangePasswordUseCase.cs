using Forum.Communication.Request;

namespace Forum.Application.UseCases.User.Change_Password
{
    public interface IChangePasswordUseCase
    {
        Task Execute(RequestChangePasswordJson request);
    }
}
