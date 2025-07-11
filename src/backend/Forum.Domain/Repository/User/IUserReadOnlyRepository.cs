﻿namespace Forum.Domain.Repository.User
{
    public interface IUserReadOnlyRepository
    {
        Task<bool> ExistActiveUserWithEmail(string email);
        Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier);
        Task<Entities.User?> GetByEmail(string email);
        Task<Entities.User?> GetByIdentifier(Guid userIdentifier);
        Task<Entities.User> GetProfile(long id);
    }
}
