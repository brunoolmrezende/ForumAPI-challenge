using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services.Photo;
using FluentAssertions;
using Forum.Application.UseCases.User.Image;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;
using Microsoft.AspNetCore.Http;
using UseCases.Test.User.InlineData;

namespace UseCases.Test.User.Image
{
    public class AddUpdateImageUseCaseTest
    {
        [Theory]
        [ClassData(typeof(ImageTypeInlineData))]
        public async Task Success(IFormFile file)
        {
            (var user, _) = UserBuilder.Build();

            var useCase = CreateUseCase(user, file);

            Func<Task> act = async () => await useCase.Execute(file);

            await act.Should().NotThrowAsync();
        }

        [Theory]
        [ClassData(typeof(ImageTypeInlineData))]
        public async Task Success_User_Without_Image(IFormFile file)
        {
            (var user, _) = UserBuilder.Build();
            user.ImageUrl = null;
            user.ImageIdentifier = null;

            var useCase = CreateUseCase(user, file);

            Func<Task> act = async () => await useCase.Execute(file);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Error_File_Is_Txt()
        {
            (var user, _) = UserBuilder.Build();

            var file = FormFileBuilder.Txt();

            var useCase = CreateUseCase(user, file);

            Func<Task> act = async () => await useCase.Execute(file);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.ONLY_IMAGES_ACCEPTED));
        }

        private static AddUpdateImageUseCase CreateUseCase(
            Forum.Domain.Entities.User user,
            IFormFile file)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var userUpdateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var photoService = new PhotoServiceBuilder().UploadImage(file, user, filename: Guid.NewGuid().ToString()).Build();

            return new AddUpdateImageUseCase(
                loggedUser,
                photoService,
                unitOfWork,
                userUpdateOnlyRepository);
        }
    }
}
