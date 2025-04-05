using Forum.Communication.Request;

namespace Forum.Application.UseCases.Topic.Update
{
    public interface IUpdateTopicUseCase
    {
        Task Execute(RequestTopicJson request, long id);
    }
}
