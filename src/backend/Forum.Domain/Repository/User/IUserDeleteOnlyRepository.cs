namespace Forum.Domain.Repository.User
{
    public interface IUserDeleteOnlyRepository
    {
        Task DeleteAccount(Guid userIdentifier);
    }
}
