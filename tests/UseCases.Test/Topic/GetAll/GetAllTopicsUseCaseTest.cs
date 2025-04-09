using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using Forum.Application.UseCases.Forum;

namespace UseCases.Test.Topic.GetAll
{
    public class GetAllTopicsUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _ ) = UserBuilder.Build();

            var topics = TopicBuilder.Collection(user);

            var useCase = CreateUseCase(topics);

            var result = await useCase.Execute();

            result.Should().NotBeNull();
            result.Topics.Should().NotBeNull();
            result.Topics.Should().HaveCount(topics.Count);
        }

        private static GetAllTopicsUseCase CreateUseCase(List<Forum.Domain.Entities.Topic> topics)
        {
            var readOnlyRepository = new TopicReadOnlyRepositoryBuilder().GetAllTopics(topics).Build();
            var mapper = MapperBuilder.Build();

            return new GetAllTopicsUseCase(readOnlyRepository, mapper);
        }
    
    } 
}
