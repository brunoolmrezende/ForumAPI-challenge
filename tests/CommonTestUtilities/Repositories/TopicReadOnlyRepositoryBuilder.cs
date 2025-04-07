using Forum.Domain.Entities;
using Forum.Domain.Repository.Topic;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class TopicReadOnlyRepositoryBuilder
    {
        private readonly Mock<ITopicReadOnlyRepository> _mock;

        public TopicReadOnlyRepositoryBuilder()
        {
            _mock = new Mock<ITopicReadOnlyRepository>();
        }

        public TopicReadOnlyRepositoryBuilder GetById(Topic? topic, User user)
        {
            if (topic is not null)
            {
                _mock.Setup(repository => repository.GetById(topic.Id, user.Id)).ReturnsAsync(topic);
            }

            return this;
        }

        public TopicReadOnlyRepositoryBuilder ExistsById(Topic? topic)
        {
            if (topic is not null)
            {
                _mock.Setup(repository => repository.ExistsById(topic.Id)).ReturnsAsync(true);
            }

            return this;
        }

        public ITopicReadOnlyRepository Build() => _mock.Object;
    }
}
