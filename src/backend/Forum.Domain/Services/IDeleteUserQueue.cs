using Forum.Domain.Entities;

namespace Forum.Domain.Services
{
    public interface IDeleteUserQueue
    {
        Task SendMessage(User user);
    }
}
