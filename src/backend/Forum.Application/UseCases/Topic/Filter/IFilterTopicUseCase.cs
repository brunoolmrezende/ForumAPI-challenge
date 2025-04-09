using Forum.Communication.Request;
using Forum.Communication.Response;

namespace Forum.Application.UseCases.Topic.Filter
{
    public interface IFilterTopicUseCase
    {
        Task<ResponseTopicsJson> Execute(RequestFilterTopicJson request);
    }
}
