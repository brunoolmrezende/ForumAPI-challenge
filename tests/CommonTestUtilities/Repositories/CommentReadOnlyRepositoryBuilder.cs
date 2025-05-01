using Forum.Domain.Repository.Comment;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class CommentReadOnlyRepositoryBuilder
    {
        private readonly Mock<ICommentReadOnlyRepository> _mock;

        public CommentReadOnlyRepositoryBuilder()
        {
            _mock = new Mock<ICommentReadOnlyRepository>();
        }

        public CommentReadOnlyRepositoryBuilder ExistsById(Forum.Domain.Entities.Comment? comment = null)
        {
            if (comment is not null)
            {
                _mock.Setup(repository => repository.ExistsById(comment.Id)).ReturnsAsync(true);
            }

            return this;
        }

        public ICommentReadOnlyRepository Build() => _mock.Object;
    }
}
