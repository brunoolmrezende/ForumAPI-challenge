using Forum.Domain.Entities;

namespace CommonTestUtilities.Entities
{
    public class TopicLikeBuilder
    {
        public static TopicLike Build(Topic topic, User user)
        {
            return new TopicLike
            {
                Id = 1,
                TopicId = topic.Id,
                UserId = user.Id,
            };
        }
    }
}
