using Forum.Communication.Response;

namespace Forum.Application.UseCases.User.Profile
{
    public interface IGetUserProfileUseCase
    {
        Task<ResponseUserProfileJson> Execute();
    }
}
