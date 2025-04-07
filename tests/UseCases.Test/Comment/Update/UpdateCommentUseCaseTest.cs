using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using Forum.Application.UseCases.Comment.Update;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace UseCases.Test.Comment.Update
{
    public class UpdateCommentUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var topic = TopicBuilder.Build(user);

            var comment = CommentBuilder.Build(user, topic.Id);

            var request = RequestCommentJsonBuilder.Build();

            var useCase = CreateUseCase(user, topic, comment);

            Func<Task> act = async () => await useCase.Execute(topic.Id, comment.Id, request);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Error_Topic_Not_Found()
        {
            (var user, _) = UserBuilder.Build();

            var topic = TopicBuilder.Build(user);

            var comment = CommentBuilder.Build(user, topic.Id);

            var request = RequestCommentJsonBuilder.Build();

            var useCase = CreateUseCase(user, topic, comment);

            Func<Task> act = async () => await useCase.Execute(1000, comment.Id, request);

            await act.Should().ThrowAsync<NotFoundException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.TOPIC_NOT_FOUND));
        }

        [Fact]
        public async Task Error_Comment_Not_Found()
        {
            (var user, _) = UserBuilder.Build();

            var topic = TopicBuilder.Build(user);

            var comment = CommentBuilder.Build(user, topic.Id);

            var request = RequestCommentJsonBuilder.Build();

            var useCase = CreateUseCase(user, topic, comment);

            Func<Task> act = async () => await useCase.Execute(topic.Id, 1000, request);

            await act.Should().ThrowAsync<NotFoundException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.COMMENT_NOT_FOUND));
        }

        [Fact]
        public async Task Error_Content_Empty()
        {
            (var user, _) = UserBuilder.Build();

            var topic = TopicBuilder.Build(user);

            var comment = CommentBuilder.Build(user, topic.Id);

            var request = RequestCommentJsonBuilder.Build();
            request.Content = string.Empty;

            var useCase = CreateUseCase(user, topic, comment);

            Func<Task> act = async () => await useCase.Execute(topic.Id, comment.Id, request);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.CONTENT_EMPTY));
        }

        private static UpdateCommentUseCase CreateUseCase(
            Forum.Domain.Entities.User user,
            Forum.Domain.Entities.Topic? topic = null,
            Forum.Domain.Entities.Comment? comment = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var unitOfWork = UnitOfWorkBuilder.Build();
            var topicReadOnlyRepository = new TopicReadOnlyRepositoryBuilder().ExistsById(topic).Build();
            var commentUpdateOnlyRepository = new CommentUpdateOnlyRepositoryBuilder().GetById(comment, topic, user.Id).Build();
            
            return new UpdateCommentUseCase(topicReadOnlyRepository, commentUpdateOnlyRepository, loggedUser, unitOfWork);
        }
    }
}
