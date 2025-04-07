using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using Forum.Application.UseCases.Comment.Delete;
using Forum.Domain.Repository.Comment;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace UseCases.Test.Comment.Delete
{
    public class DeleteCommentUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _ ) = UserBuilder.Build();

            var topic = TopicBuilder.Build(user);

            var comment = CommentBuilder.Build(user, topic.Id);

            var useCase = CreateUseCase(user, topic, comment);

            Func<Task> act = async () => await useCase.Execute(topic.Id, comment.Id);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Error_Topic_Not_Found()
        {
            (var user, _) = UserBuilder.Build();

            var topic = TopicBuilder.Build(user);

            var comment = CommentBuilder.Build(user, topic.Id);

            var useCase = CreateUseCase(user: user, comment: comment);

            Func<Task> act = async () => await useCase.Execute(1000, comment.Id);

            await act.Should().ThrowAsync<NotFoundException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.TOPIC_NOT_FOUND));
        }

        [Fact]
        public async Task Error_Comment_Not_Found()
        {
            (var user, _) = UserBuilder.Build();

            var topic = TopicBuilder.Build(user);

            var useCase = CreateUseCase(user, topic);

            Func<Task> act = async () => await useCase.Execute(topic.Id, 1000);

            await act.Should().ThrowAsync<NotFoundException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.COMMENT_NOT_FOUND));
        }

        private static DeleteCommentUseCase CreateUseCase(
            Forum.Domain.Entities.User user,
            Forum.Domain.Entities.Topic? topic = null,
            Forum.Domain.Entities.Comment? comment = null)
        {
            var topicReadOnlyRepository = new TopicReadOnlyRepositoryBuilder().ExistsById(topic).Build();
            var commentUpdateOnlyRepository = new CommentUpdateOnlyRepositoryBuilder().GetById(comment, topic, user.Id).Build();
            var commentWriteOnlyRepository = CommentWriteOnlyRepositoryBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new DeleteCommentUseCase(topicReadOnlyRepository, commentUpdateOnlyRepository, commentWriteOnlyRepository, loggedUser, unitOfWork);
        }
    }
}
