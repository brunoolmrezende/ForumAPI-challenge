namespace Forum.Domain.Repository.Like.TopicLike
{
    public interface ITopicLikeUpdateOnlyRepository
    {
        Task<Entities.TopicLike?> GetById(long userId, long topicId);
    }
}
