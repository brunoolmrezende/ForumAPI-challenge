namespace Forum.Domain.Repository.Comment
{
    public interface ICommentWriteOnlyRepository
    {
        Task Add(Entities.Comment comment);
        void Delete(Entities.Comment comment);
    }
}
