namespace Forum.Domain.Repository.Topic
{
    public interface ITopicReadOnlyRepository
    {
        Task<Entities.Topic?> GetById(long id, long loggedUserId);
        Task<Entities.Topic?> GetTopicDetails(long id);
        Task<bool> ExistsById(long topicId);
        Task<List<Entities.Topic>> GetAllTopics();
    }
}
