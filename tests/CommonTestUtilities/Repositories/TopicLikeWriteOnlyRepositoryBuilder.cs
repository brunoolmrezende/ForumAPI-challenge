using Forum.Domain.Repository.Like.TopicLike;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class TopicLikeWriteOnlyRepositoryBuilder
    {
        public static ITopicLikeWriteOnlyRepository Build()
        {
            var mock = new Mock<ITopicLikeWriteOnlyRepository>();

            return mock.Object;
        }
    }
}
