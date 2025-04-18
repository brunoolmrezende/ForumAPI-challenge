using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using Forum.Application.UseCases.Token;
using Forum.Communication.Request;
using Forum.Domain.ValueObjects;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace UseCases.Test.Token
{
    public class UseRefreshTokenUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _ ) = UserBuilder.Build();

            var refreshToken = RefreshTokenBuilder.Build(user);

            var useCase = CreateUseCase(refreshToken);

            var result = await useCase.Execute(new RequestNewTokenJson
            {
                RefreshToken = refreshToken.Value
            });

            result.Should().NotBeNull();
            result.RefreshToken.Should().NotBeNullOrEmpty();
            result.AccessToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Error_Refresh_Token_Not_Found()
        {
            (var user, _) = UserBuilder.Build();

            var refreshToken = RefreshTokenBuilder.Build(user);

            var useCase = CreateUseCase();

            Func<Task> act = async () => await useCase.Execute(new RequestNewTokenJson
            {
                RefreshToken = refreshToken.Value
            });

            await act.Should().ThrowAsync<RefreshTokenNotFoundException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.TOKEN_NOT_FOUND));
        }

        [Fact]
        public async Task Error_Refresh_Token_Expired()
        {
            (var user, _) = UserBuilder.Build();
            var refreshToken = RefreshTokenBuilder.Build(user);
            refreshToken.CreatedOn = DateTime.UtcNow.AddHours(-ForumRuleConstants.MAXIMUM_REFRESH_TOKEN_TIME_IN_HOURS);

            var useCase = CreateUseCase(refreshToken);

            var act = async () => await useCase.Execute(new RequestNewTokenJson
            {
                RefreshToken = refreshToken.Value,
            });

            await act.Should().ThrowAsync<RefreshTokenExpiredException>()
                .Where(error => error.GetErrorMessage().Count == 1 
                    && error.GetErrorMessage().Contains(ResourceMessagesException.EXPIRED_TOKEN));
        }

        private static UseRerfreshTokenUseCase CreateUseCase(Forum.Domain.Entities.RefreshToken? refreshToken = null)
        {
            var tokenRepository = new TokenRepositoryBuilder().GetToken(refreshToken).Build() ;
            var refreshTokenGenerator = RefreshTokenGeneratorBuilder.Build();
            var accessTokenGenerator = AccessTokenGeneratorBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new UseRerfreshTokenUseCase(tokenRepository, refreshTokenGenerator, accessTokenGenerator, unitOfWork);
        }
    }
}
