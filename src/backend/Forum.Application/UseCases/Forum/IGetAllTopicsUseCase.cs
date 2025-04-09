using Forum.Communication.Response;

namespace Forum.Application.UseCases.Forum
{
    public interface IGetAllTopicsUseCase
    {
        Task<ResponseTopicsJson> Execute();
    }
}
