using Forum.Communication.Request;
using Forum.Communication.Response;

namespace Forum.Application.UseCases.Topic.Register
{
    public interface IRegisterTopicUseCase
    {
        Task<ResponseRegisteredTopicJson> Execute(RequestRegisterTopicJson request);
    }
}
