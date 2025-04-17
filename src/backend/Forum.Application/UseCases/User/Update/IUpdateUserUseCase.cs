using Forum.Communication.Request;

namespace Forum.Application.UseCases.User.Update
{
    public interface IUpdateUserUseCase
    {
        Task Execute(RequestUpdateUserJson request);
    }
}