using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using Forum.Application.UseCases.Like.Comment;
using Forum.Exceptions.ExceptionBase;
using Forum.Exceptions;

namespace UseCases.Test.Like.TopicLike
{
    public class ToggleCommentLikeUseCaseTest
    {
        [Fact]
        public async Task Success_Like()
        {
            (var user, _) = UserBuilder.Build();

            var topic = TopicBuilder.Build(user);

            var comment = CommentBuilder.Build(user, topic.Id);

            var commentLike = CommentLikeBuilder.Build(comment, user);

            var useCase = CreateUseCase(user, comment, commentLike);

            Func<Task> act = async () => await useCase.Execute(comment.Id);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Success_Unlike()
        {
            (var user, _) = UserBuilder.Build();

            var topic = TopicBuilder.Build(user);

            var comment = CommentBuilder.Build(user, topic.Id);

            var useCase = CreateUseCase(user, comment);

            Func<Task> act = async () => await useCase.Execute(topic.Id);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Error_Comment_NotFound()
        {
            (var user, _) = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(commentId: 1000);

            await act.Should().ThrowAsync<NotFoundException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.COMMENT_NOT_FOUND));
        }

        private static ToggleCommentLikeUseCase CreateUseCase(
            Forum.Domain.Entities.User user,
            Forum.Domain.Entities.Comment? comment = null,
            Forum.Domain.Entities.CommentLike? commentLike = null)
        {
            var commentReadOnlyRepository = new CommentReadOnlyRepositoryBuilder().ExistsById(comment).Build();
            var commentLikeUpdateOnlyRepository = new CommentLikeUpdateOnlyRepositoryBuilder().GetById(user, commentLike).Build();
            var commentLikeWriteOnlyRepository = CommentLikeWriteOnlyRepositoryBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new ToggleCommentLikeUseCase(
                loggedUser,
                commentReadOnlyRepository,
                commentLikeUpdateOnlyRepository,
                commentLikeWriteOnlyRepository,
                unitOfWork);
        }
    }
}
