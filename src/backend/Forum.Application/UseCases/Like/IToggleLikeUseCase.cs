namespace Forum.Application.UseCases.Like
{
    public interface IToggleLikeUseCase
    {
        Task Execute(long topicId);
    }
}
