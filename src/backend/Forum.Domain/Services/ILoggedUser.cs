using Forum.Domain.Entities;

namespace Forum.Domain.Services
{
    public interface ILoggedUser
    {
        Task<User> User();
    }
}
