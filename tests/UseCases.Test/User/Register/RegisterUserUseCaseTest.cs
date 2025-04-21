using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services.Photo;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using Forum.Application.UseCases.User.Register;
using Forum.Domain.Entities;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;
using Microsoft.AspNetCore.Http;
using UseCases.Test.User.InlineData;

namespace UseCases.Test.User.Register
{
    public class RegisterUserUseCaseTest
    {
        [Fact]
        public async Task Success_Without_Image()
        {
            var request = RequestRegisterUserFormDataBuilder.Build();

            (var user, _) = UserBuilder.Build();
            user.Name = request.Name;
            user.Email = request.Email;

            var useCase = CreateUseCase(user);

            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Name.Should().Be(request.Name);
            result.Tokens.AccessToken.Should().NotBeNullOrEmpty();
            result.Tokens.RefreshToken.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [ClassData(typeof(ImageTypeInlineData))]
        public async Task Success_With_Image(IFormFile file)
        {
            var request = RequestRegisterUserFormDataBuilder.Build(file);

            (var user, _ ) = UserBuilder.Build();
            user.Name = request.Name;
            user.Email = request.Email;

            var useCase = CreateUseCase(user, file: file);

            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Name.Should().Be(request.Name);
            result.Tokens.AccessToken.Should().NotBeNullOrEmpty();
            result.Tokens.RefreshToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Error_Email_Already_Registered()
        {
            var request = RequestRegisterUserFormDataBuilder.Build();

            (var user, _) = UserBuilder.Build();
            user.Name = request.Name;
            user.Email = request.Email;

            var useCase = CreateUseCase(user, request.Email);

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            var request = RequestRegisterUserFormDataBuilder.Build();
            request.Name = string.Empty;

            (var user, _) = UserBuilder.Build();
            user.Name = request.Name;
            user.Email = request.Email;

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.NAME_EMPTY));
        }

        private static RegisterUserUseCase CreateUseCase(
            Forum.Domain.Entities.User user,
            string? email = null, 
            RefreshToken? refreshToken = null,
            IFormFile? file = null)
        {
            var mapper = MapperBuilder.Build();
            var userWriteOnlyRepository = new UserWriteOnlyRepositoryBuilder().Build();
            var encryption = PasswordEncryptionBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();
            var tokenRepository = new TokenRepositoryBuilder().GetToken(refreshToken).Build();
            var refreshTokenGenerator = RefreshTokenGeneratorBuilder.Build();
            var photoService = new PhotoServiceBuilder().UploadImage(file, user , filename: Guid.NewGuid().ToString()).Build();

            if (!string.IsNullOrWhiteSpace(email))
            {
                userReadOnlyRepository.ExistActiveUserWithEmail(email);
            }

            var accessToken = AccessTokenGeneratorBuilder.Build();

            return new RegisterUserUseCase(
                mapper, 
                userWriteOnlyRepository, 
                encryption, 
                unitOfWork, 
                userReadOnlyRepository.Build(), 
                accessToken, 
                refreshTokenGenerator, 
                tokenRepository,
                photoService);
        }
    }
}
