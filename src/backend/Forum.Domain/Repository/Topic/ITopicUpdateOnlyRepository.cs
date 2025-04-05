namespace Forum.Domain.Repository.Topic
{
    public interface ITopicUpdateOnlyRepository
    {
        Task<Entities.Topic?> GetById(long id, long loggedUserId);
        void Update(Entities.Topic topic);
    }
}
