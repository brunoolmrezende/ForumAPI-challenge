using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using Forum.Application.UseCases.Topic.Delete;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace UseCases.Test.Topic.Delete
{
    public class DeleteTopicUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var topic = TopicBuilder.Build(user);

            var useCase = CreateUseCase(user, topic);

            Func<Task> act = async () => await useCase.Execute(topic.Id);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Error_Topic_Not_Found()
        {
            (var user, _) = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(id: 1);

            await act.Should().ThrowAsync<NotFoundException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.TOPIC_NOT_FOUND));
        }

        private DeleteTopicUseCase CreateUseCase(
            Forum.Domain.Entities.User user, 
            Forum.Domain.Entities.Topic? topic = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var readOnlyRepository = new TopicReadOnlyRepositoryBuilder().GetById(topic, user).Build();
            var writeOnlyRepository = TopicWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new DeleteTopicUseCase(loggedUser, readOnlyRepository, writeOnlyRepository, unitOfWork);
        }
    }
}
