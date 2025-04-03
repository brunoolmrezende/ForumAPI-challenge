namespace Forum.Domain.Repository.User
{
    public interface IUserReadOnlyRepository
    {
        Task<bool> ExistActiveUserWithEmail(string email);
        Task<Entities.User?> GetByEmail(string email);
    }
}
