using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using Forum.Application.UseCases.Topic.Filter;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace UseCases.Test.Topic.Filter
{
    public class FilterTopicUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var request = RequestFilterTopicJsonBuilder.Build();

            (var user, _) = UserBuilder.Build();

            var topics = TopicBuilder.Collection(user);

            var useCase = CreateUseCase(topics);

            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Topics.Should().NotBeNullOrEmpty();
            result.Topics.Should().HaveCount(topics.Count);
        }

        [Fact]
        public async Task Error_Title_Filter_Length()
        {
            var request = RequestFilterTopicJsonBuilder.Build();
            request.Title = new string('a', 256);

            (var user, _) = UserBuilder.Build();

            var topics = TopicBuilder.Collection(user);

            var useCase = CreateUseCase(topics);

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.TITLE_MAX_LENGTH));
        }

        private static FilterTopicUseCase CreateUseCase(List<Forum.Domain.Entities.Topic> topics)
        {
            var repository = new TopicReadOnlyRepositoryBuilder().Filter(topics).Build();
            var mapper = MapperBuilder.Build();

            return new FilterTopicUseCase(repository, mapper);
        }
    }
}
