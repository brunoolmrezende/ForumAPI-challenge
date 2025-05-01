using Forum.Domain.Repository.Like.TopicLike;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class TopicLikeUpdateOnlyRepositoryBuilder
    {
        private readonly Mock<ITopicLikeUpdateOnlyRepository> _mock;

        public TopicLikeUpdateOnlyRepositoryBuilder()
        {
            _mock = new Mock<ITopicLikeUpdateOnlyRepository>();
        }

        public TopicLikeUpdateOnlyRepositoryBuilder GetById(
            Forum.Domain.Entities.User user,
            Forum.Domain.Entities.TopicLike? topicLike)
        {
            if (topicLike is not null)
            {
                _mock.Setup(repository => repository.GetById(user.Id, topicLike.TopicId)).ReturnsAsync(topicLike);
            }

            return this;
        }

        public ITopicLikeUpdateOnlyRepository Build() => _mock.Object;
    }
}
