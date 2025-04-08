using Forum.Communication.Response;

namespace Forum.Application.UseCases.Topic.GetById
{
    public interface IGetTopicByIdUseCase
    {
        Task<ResponseTopicDetailsJson> Execute(long id);
    }
}
