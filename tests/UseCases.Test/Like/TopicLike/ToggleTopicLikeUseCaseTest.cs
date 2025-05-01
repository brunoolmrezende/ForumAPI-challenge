using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using Forum.Application.UseCases.Like.Topic;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace UseCases.Test.Like.ToggleLike
{
    public class ToggleTopicLikeUseCaseTest
    {
        [Fact]
        public async Task Success_Like()
        {
            (var user, _) = UserBuilder.Build();

            var topic = TopicBuilder.Build(user);

            var topicLike = TopicLikeBuilder.Build(topic, user);

            var useCase = CreateUseCase(user, topic, topicLike);

            Func<Task> act = async () => await useCase.Execute(topic.Id);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Success_Unlike()
        {
            (var user, _) = UserBuilder.Build();

            var topic = TopicBuilder.Build(user);

            var useCase = CreateUseCase(user, topic);

            Func<Task> act = async () => await useCase.Execute(topic.Id);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Error_Topic_NotFound()
        {
            (var user, _) = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(topicId: 1000);

            await act.Should().ThrowAsync<NotFoundException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.TOPIC_NOT_FOUND));
        }

        private static ToggleTopicLikeUseCase CreateUseCase(
            Forum.Domain.Entities.User user,
            Forum.Domain.Entities.Topic? topic = null,
            Forum.Domain.Entities.TopicLike? topicLike = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var topicReadOnlyRepository = new TopicReadOnlyRepositoryBuilder().ExistsById(topic).Build();
            var topicLikeUpdateOnlyRepository = new TopicLikeUpdateOnlyRepositoryBuilder().GetById(user, topicLike).Build();
            var topicLikeWriteOnlyRepository = TopicLikeWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new ToggleTopicLikeUseCase(loggedUser, topicReadOnlyRepository, topicLikeUpdateOnlyRepository, topicLikeWriteOnlyRepository, unitOfWork);
        }
    }
}
