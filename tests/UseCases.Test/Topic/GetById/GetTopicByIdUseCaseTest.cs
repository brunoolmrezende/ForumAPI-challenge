using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using Forum.Application.UseCases.Topic.GetById;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace UseCases.Test.Topic.GetById
{
    public class GetTopicByIdUseCaseTest
    {
        [Fact]
        public async Task Success_Without_Auth()
        {
            (var user, _) = UserBuilder.Build();

            var topic = TopicBuilder.Build(user);
            var comment = CommentBuilder.Build(user, topic.Id);
            var topicLike = TopicLikeBuilder.Build(topic, user);

            topic.Comments.Add(comment);
            topic.Likes.Add(topicLike);
            topic.User = user;

            var useCase = CreateUseCase(topic);

            var result = await useCase.Execute(topic.Id);

            result.Should().NotBeNull();
            result.Id.Should().Be(topic.Id);
            result.Title.Should().Be(topic.Title);
            result.Content.Should().Be(topic.Content);
            result.CreatedOn.Should().Be(topic.CreatedOn);

            result.Comments.Should().HaveCount(1);
            result.Comments.Should().AllSatisfy(c =>
            {
                c.Content.Should().Be(comment.Content);
                c.Id.Should().Be(comment.Id);
            });

            result.User.Should().NotBeNull();
            result.User.Id.Should().Be(user.Id);
            result.User.Name.Should().Be(user.Name);

            result.TotalLikes.Should().Be(topic.Likes.Count);
            result.LikedByCurrentUser.Should().BeFalse();
        }

        [Fact]
        public async Task Success_With_Auth()
        {
            (var user, _) = UserBuilder.Build();

            var topic = TopicBuilder.Build(user);
            var comment = CommentBuilder.Build(user, topic.Id);
            var topicLike = TopicLikeBuilder.Build(topic, user);

            topic.Comments.Add(comment);
            topic.Likes.Add(topicLike);
            topic.User = user;

            var useCase = CreateUseCase(topic, user);

            var result = await useCase.Execute(topic.Id);

            result.Should().NotBeNull();
            result.Id.Should().Be(topic.Id);
            result.Title.Should().Be(topic.Title);
            result.Content.Should().Be(topic.Content);
            result.CreatedOn.Should().Be(topic.CreatedOn);

            result.Comments.Should().HaveCount(1);
            result.Comments.Should().AllSatisfy(c =>
            {
                c.Content.Should().Be(comment.Content);
                c.Id.Should().Be(comment.Id);
            });

            result.User.Should().NotBeNull();
            result.User.Id.Should().Be(user.Id);
            result.User.Name.Should().Be(user.Name);

            result.TotalLikes.Should().Be(topic.Likes.Count);
            result.LikedByCurrentUser.Should().BeTrue();
        }

        [Fact]
        public async Task Error_Topic_Not_Found()
        {
            (var user, _) = UserBuilder.Build();

            var topic = TopicBuilder.Build(user);
            var comment = CommentBuilder.Build(user, topic.Id);
            var topicLike = TopicLikeBuilder.Build(topic, user);

            topic.Comments.Add(comment);
            topic.Likes.Add(topicLike);

            var useCase = CreateUseCase();

            Func<Task> act = async () => await useCase.Execute(topic.Id);

            await act.Should().ThrowAsync<NotFoundException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.TOPIC_NOT_FOUND));

        }

        private static GetTopicByIdUseCase CreateUseCase(
            Forum.Domain.Entities.Topic? topic = null,
            Forum.Domain.Entities.User? user = null)
        {
            var readOnlyRepository = new TopicReadOnlyRepositoryBuilder().GetTopicDetails(topic).Build();
            var loggedUser = LoggedUserBuilder.BuildTryGetUser(user);
            var mapper = MapperBuilder.Build();

            return new GetTopicByIdUseCase(readOnlyRepository, loggedUser, mapper);
        }
    }
}
