using Forum.Domain.Repository.Like.CommentLike;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public static class CommentLikeWriteOnlyRepositoryBuilder
    {
        public static ICommentLikeWriteOnlyRepository Build()
        {
            var mock = new Mock<ICommentLikeWriteOnlyRepository>();

            return mock.Object;
        }
    }
}
