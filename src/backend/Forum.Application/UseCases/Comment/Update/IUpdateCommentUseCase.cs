using Forum.Communication.Request;

namespace Forum.Application.UseCases.Comment.Update
{
    public interface IUpdateCommentUseCase
    {
        Task Execute(long topicId, long commentId, RequestCommentJson request);
    }
}
