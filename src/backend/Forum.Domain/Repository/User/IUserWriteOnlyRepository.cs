namespace Forum.Domain.Repository.User
{
    public interface IUserWriteOnlyRepository
    {
        Task Add(Entities.User user);
    }
}
