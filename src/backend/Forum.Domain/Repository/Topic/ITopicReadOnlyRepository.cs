namespace Forum.Domain.Repository.Topic
{
    public interface ITopicReadOnlyRepository
    {
        Task<Entities.Topic?> GetById(long id, long loggedUserId);
    }
}
