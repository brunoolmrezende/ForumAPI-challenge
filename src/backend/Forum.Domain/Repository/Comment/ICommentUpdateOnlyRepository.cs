namespace Forum.Domain.Repository.Comment
{
    public interface ICommentUpdateOnlyRepository
    {
        Task<Entities.Comment?> GetById(long commentId, long loggedUserId);
        void Update(Entities.Comment comment);
    }
}
