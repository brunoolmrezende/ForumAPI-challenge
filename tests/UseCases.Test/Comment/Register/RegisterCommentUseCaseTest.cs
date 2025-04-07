using Bogus;
using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using Forum.Application.UseCases.Comment.Register;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace UseCases.Test.Comment.Register
{
    public class RegisterCommentUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var topic = TopicBuilder.Build(user);
            
            var request = RequestCommentJsonBuilder.Build();

            var useCase = CreateUseCase(user, topic);

            var result = await useCase.Execute(topic.Id, request);

            result.Content.Should().Be(request.Content);
            result.Id.Should().BeGreaterThanOrEqualTo(0);
        }

        [Fact]
        public async Task Error_Content_Empty()
        {
            (var user, _) = UserBuilder.Build();

            var topic = TopicBuilder.Build(user);

            var request = RequestCommentJsonBuilder.Build();
            request.Content = string.Empty;

            var useCase = CreateUseCase(user, topic);

            Func<Task> act = async () => await useCase.Execute(topic.Id, request);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.CONTENT_EMPTY));
        }

        [Fact]
        public async Task Error_Content_Max_Length()
        {
            (var user, _) = UserBuilder.Build();

            var topic = TopicBuilder.Build(user);

            var request = RequestCommentJsonBuilder.Build();
            request.Content = new Faker().Lorem.Letter(2001);

            var useCase = CreateUseCase(user, topic);

            Func<Task> act = async () => await useCase.Execute(topic.Id, request);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.CONTENT_MAX_LENGTH));
        }

        [Fact]
        public async Task Error_Topic_Not_Found()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestCommentJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(topicId: 1, request);

            await act.Should().ThrowAsync<NotFoundException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.TOPIC_NOT_FOUND));
        }

        private static RegisterCommentUseCase CreateUseCase(
            Forum.Domain.Entities.User user,
            Forum.Domain.Entities.Topic? topic = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var topicReadOnlyRepository = new TopicReadOnlyRepositoryBuilder().ExistsById(topic).Build();
            var commentWriteOnlyRepository = CommentWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var mapper = MapperBuilder.Build();

            return new RegisterCommentUseCase(
                loggedUser, topicReadOnlyRepository, commentWriteOnlyRepository, unitOfWork, mapper);
        }
    }
}
