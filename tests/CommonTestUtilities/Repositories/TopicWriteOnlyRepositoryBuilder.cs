using Forum.Domain.Repository.Like.TopicLike;
using Forum.Domain.Repository.Topic;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class TopicWriteOnlyRepositoryBuilder
    {
        public static ITopicWriteOnlyRepository Build()
        {
            var mock = new Mock<ITopicWriteOnlyRepository>();

            return mock.Object;
        }
    }
}
