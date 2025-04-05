using Forum.Domain.Repository.Topic;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class TopicUpdateOnlyRepositoryBuilder
    {
        private readonly Mock<ITopicUpdateOnlyRepository> _mock;

        public TopicUpdateOnlyRepositoryBuilder()
        {
            _mock = new Mock<ITopicUpdateOnlyRepository>();
        }

        public TopicUpdateOnlyRepositoryBuilder GetById(Forum.Domain.Entities.Topic topic, long userId)
        {
            if (topic is not null)
            {
                _mock.Setup(respository => respository.GetById(It.IsAny<long>(), userId)).ReturnsAsync(topic);
            }

            return this;
        }

        public ITopicUpdateOnlyRepository Build() => _mock.Object;
    }
}
