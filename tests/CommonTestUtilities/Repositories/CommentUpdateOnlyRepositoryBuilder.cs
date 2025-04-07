using Forum.Domain.Repository.Comment;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class CommentUpdateOnlyRepositoryBuilder
    {
        private readonly Mock<ICommentUpdateOnlyRepository> _mock;

        public CommentUpdateOnlyRepositoryBuilder()
        {
            _mock = new Mock<ICommentUpdateOnlyRepository>();
        }

        public CommentUpdateOnlyRepositoryBuilder GetById(
            Forum.Domain.Entities.Comment? comment,
            Forum.Domain.Entities.Topic? topic,
            long loggedUserId)
        {
            if (comment is not null && topic is not null)
            {
                _mock.Setup(x => x.GetById(comment.Id, loggedUserId, topic.Id)).ReturnsAsync(comment);
            }

            return this;
        }

        public ICommentUpdateOnlyRepository Build() => _mock.Object;
    }
}
