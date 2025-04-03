namespace Forum.Domain.Repository.User
{
    public interface IUserRepository
    {
        Task Add(Entities.User user);
    }
}
