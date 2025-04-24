using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services.Photo;
using FluentAssertions;
using Forum.Application.UseCases.User.Delete.Delete_User_Account;

namespace UseCases.Test.User.Delete
{
    public class DeleteUserAccountUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> action = async () => await useCase.Execute(user.UserIdentifier);

            await action.Should().NotThrowAsync();
        }

        private static DeleteUserAccountUseCase CreateUseCase(Forum.Domain.Entities.User user)
        {
            var deleteOnlyRepository = new UserDeleteOnlyRepositoryBuilder().Build();
            var readOnlyRepository = new UserReadOnlyRepositoryBuilder().GetByIdentifier(user).Build();
            var photoService = new PhotoServiceBuilder().Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new DeleteUserAccountUseCase(
                deleteOnlyRepository,
                readOnlyRepository,
                photoService,
                unitOfWork);
        }
    }
}
