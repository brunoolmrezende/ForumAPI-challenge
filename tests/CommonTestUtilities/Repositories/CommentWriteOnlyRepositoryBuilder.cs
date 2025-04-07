using Forum.Domain.Repository.Comment;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class CommentWriteOnlyRepositoryBuilder
    {
        public static ICommentWriteOnlyRepository Build()
        {
            var mock = new Mock<ICommentWriteOnlyRepository>();

            return mock.Object;
        }
    }
}
