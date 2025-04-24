using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services.Queue;
using FluentAssertions;
using Forum.Application.UseCases.User.Delete.Request;

namespace UseCases.Test.User.Delete.Request
{
    public class RequestDeleteAccountUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _ ) = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute();

            await act.Should().NotThrowAsync();
            user.Active.Should().BeFalse();
        }

        private RequestDeleteUserUseCase CreateUseCase(Forum.Domain.Entities.User user)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var repository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var deleteUserQueue = DeleteUserQueueBuilder.Build();

            return new RequestDeleteUserUseCase(
                loggedUser,
                repository,
                unitOfWork,
                deleteUserQueue);
        }
    }
}
