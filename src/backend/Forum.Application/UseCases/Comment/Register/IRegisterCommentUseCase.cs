using Forum.Communication.Request;
using Forum.Communication.Response;

namespace Forum.Application.UseCases.Comment.Register
{
    public interface IRegisterCommentUseCase
    {
        Task<ResponseRegisteredCommentJson> Execute(long topicId, RequestCommentJson request);
    }
}
