using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using Forum.Application.UseCases.Topic.Update;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace UseCases.Test.Topic.Update
{
    public class UpdateTopicUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var topic = TopicBuilder.Build(user);

            var request = RequestTopicJsonBuilder.Build();

            var useCase = CreateUseCase(user, topic);

            Func<Task> act = async () => await useCase.Execute(request, topic.Id);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Error_Topic_Not_Found()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestTopicJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(request, 1000);

            await act.Should().ThrowAsync<NotFoundException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.TOPIC_NOT_FOUND));
        }

        private static UpdateTopicUseCase CreateUseCase(
            Forum.Domain.Entities.User user, 
            Forum.Domain.Entities.Topic? topic = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var mapper = MapperBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var updateOnlyRepository = new TopicUpdateOnlyRepositoryBuilder();

            if (topic is not null)
            {
                updateOnlyRepository.GetById(topic, user.Id);
            }

            return new UpdateTopicUseCase(loggedUser, updateOnlyRepository.Build(), mapper, unitOfWork);
        }
    }
}
