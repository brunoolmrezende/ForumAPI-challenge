namespace Forum.Domain.Repository
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
