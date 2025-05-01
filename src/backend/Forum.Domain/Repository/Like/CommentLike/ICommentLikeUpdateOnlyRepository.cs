namespace Forum.Domain.Repository.Like.CommentLike
{
    public interface ICommentLikeUpdateOnlyRepository
    {
        Task<Entities.CommentLike?> GetById(long userId, long commentId);
    }
}
