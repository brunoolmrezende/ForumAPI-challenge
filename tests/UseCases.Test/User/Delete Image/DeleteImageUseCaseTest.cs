using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services.Photo;
using FluentAssertions;
using Forum.Application.UseCases.User.Delete_Image;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace UseCases.Test.User.Delete_Image
{
    public class DeleteImageUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute();

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Error_User_Does_Not_Have_Photo()
        {
            (var user, _) = UserBuilder.Build();
            user.ImageIdentifier = null;
            user.ImageUrl = null;

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute();

            await act.Should().ThrowAsync<NotFoundException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.USER_DOES_NOT_HAVE_PHOTO));
        }

        private static DeleteImageUseCase CreateUseCase(Forum.Domain.Entities.User user)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var userUpdateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var photoService = new PhotoServiceBuilder().Build();

            return new DeleteImageUseCase(loggedUser, userUpdateOnlyRepository, photoService, unitOfWork);
        }
    }
}
