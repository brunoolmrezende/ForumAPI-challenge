namespace Forum.Domain.Repository.Like.TopicLike
{
    public interface ITopicLikeWriteOnlyRepository
    {
        Task Add(Entities.TopicLike topicLike);
        void Delete(Entities.TopicLike topicLike);
    }
}
