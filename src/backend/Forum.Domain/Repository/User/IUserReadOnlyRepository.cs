namespace Forum.Domain.Repository.User
{
    public interface IUserReadOnlyRepository
    {
        Task<bool> ExistActiveUserWithEmail(string email);
    }
}
