using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using Forum.Application.UseCases.Login.DoLogin;
using Forum.Communication.Request;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;


namespace UseCases.Test.Login.DoLogin
{
    public class DoLoginUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, var password) = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            var result = await useCase.Execute(new RequestDoLoginJson
            {
                Email = user.Email,
                Password = password
            });

            result.Should().NotBeNull();
            result.Name.Should().Be(user.Name);
            result.Tokens.AccessToken.Should().NotBeNullOrEmpty();
            result.Tokens.RefreshToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Error_Invalid_Email_Or_Pasword()
        {
            var request = RequestDoLoginJsonBuilder.Build();

            var useCase = CreateUseCase();

            Func<Task> act = async () => await useCase.Execute(request);
            await act.Should().ThrowAsync<InvalidLoginException>()
                .Where(error => error.GetErrorMessage().Contains(ResourceMessagesException.INVALID_EMAIL_OR_PASSWORD));
        }

        private static DoLoginUseCase CreateUseCase(
            Forum.Domain.Entities.User? user = null,
            Forum.Domain.Entities.RefreshToken? refreshToken = null)
        {
            var readOnlyRepository = new UserReadOnlyRepositoryBuilder();
            var encryption = PasswordEncryptionBuilder.Build();
            var tokenRepository = new TokenRepositoryBuilder().GetToken(refreshToken).Build();
            var refreshTokenGenerator = RefreshTokenGeneratorBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            if (user is not null)
            {
                readOnlyRepository.GetByEmail(user);
            }

            var accessToken = AccessTokenGeneratorBuilder.Build();

            return new DoLoginUseCase(
                readOnlyRepository.Build(), 
                encryption, 
                accessToken, 
                refreshTokenGenerator, 
                tokenRepository, 
                unitOfWork);
        }
    }
}
