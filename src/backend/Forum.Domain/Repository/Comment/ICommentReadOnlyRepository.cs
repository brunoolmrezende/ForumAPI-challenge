namespace Forum.Domain.Repository.Comment
{
    public interface ICommentReadOnlyRepository
    {
        Task<bool> ExistsById(long commentId);
    }
}
