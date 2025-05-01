using Forum.Domain.Entities;

namespace CommonTestUtilities.Entities
{
    public static class CommentLikeBuilder
    {
        public static CommentLike Build(Comment comment, User user)
        {
            return new CommentLike
            {
                Id = 1,
                CommentId = comment.Id,
                UserId = user.Id,
            };
        }
    }
}
