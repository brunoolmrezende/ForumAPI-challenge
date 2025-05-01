namespace Forum.Domain.Repository.Like.CommentLike
{
    public interface ICommentLikeWriteOnlyRepository
    {
        Task Add(Entities.CommentLike commentLike);
        void Delete(Entities.CommentLike commentLike);
    }
}
