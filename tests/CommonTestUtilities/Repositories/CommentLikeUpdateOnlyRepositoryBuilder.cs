using Forum.Domain.Repository.Like.CommentLike;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class CommentLikeUpdateOnlyRepositoryBuilder
    {
        private readonly Mock<ICommentLikeUpdateOnlyRepository> _mock;

        public CommentLikeUpdateOnlyRepositoryBuilder()
        {
            _mock = new Mock<ICommentLikeUpdateOnlyRepository>();
        }

        public CommentLikeUpdateOnlyRepositoryBuilder GetById(
            Forum.Domain.Entities.User user,
            Forum.Domain.Entities.CommentLike? commentLike = null)
        {
            if (commentLike is not null)
            {
                _mock.Setup(repository => repository.GetById(user.Id, commentLike.Id)).ReturnsAsync(commentLike);
            }

            return this;
        }

        public ICommentLikeUpdateOnlyRepository Build() => _mock.Object;
    }
}
