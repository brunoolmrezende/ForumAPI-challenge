namespace Forum.Application.UseCases.Topic.Delete
{
    public interface IDeleteTopicUseCase
    {
        Task Execute(long id);
    }
}
