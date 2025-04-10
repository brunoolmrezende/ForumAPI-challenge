﻿namespace Forum.Domain.Repository.Topic
{
    public interface ITopicWriteOnlyRepository
    {
        Task Add(Entities.Topic topic);
        Task Delete(long id);
    }
}
