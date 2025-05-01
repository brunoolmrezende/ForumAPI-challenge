namespace Forum.Application.UseCases.Like.Topic
{
    public interface IToggleTopicLikeUseCase
    {
        Task Execute(long topicId);
    }
}
