using Forum.Communication.Request;
using Forum.Communication.Response;
using Forum.Domain.Entities;
using Forum.Domain.Repository;
using Forum.Domain.Repository.Token;
using Forum.Domain.Security.AccessToken;
using Forum.Domain.Security.RefreshToken;
using Forum.Domain.ValueObjects;
using Forum.Exceptions.ExceptionBase;

namespace Forum.Application.UseCases.Token
{
    public class UseRerfreshTokenUseCase(
        ITokenRepository tokenRepository,
        IRefreshTokenGenerator refreshTokenGenerator,
        IAccessTokenGenerator accessTokenGenerator,
        IUnitOfWork unitOfWork) : IUseRefreshTokenUseCase
    {
        private readonly ITokenRepository _tokenRepository = tokenRepository;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator = refreshTokenGenerator;
        private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;


        public async Task<ResponseTokensJson> Execute(RequestNewTokenJson request)
        {
            var refreshToken = await _tokenRepository.GetToken(request.RefreshToken);

            if (refreshToken is null)
            {
                throw new RefreshTokenNotFoundException();
            }

            var refreshTokenTime = refreshToken.CreatedOn.AddHours(ForumRuleConstants.MAXIMUM_REFRESH_TOKEN_TIME_IN_HOURS);

            if (DateTime.Compare(refreshTokenTime, DateTime.UtcNow) < 0)
            {
                throw new RefreshTokenExpiredException();
            }

            var newRefreshToken = new RefreshToken
            {
                UserId = refreshToken.UserId,
                Value = _refreshTokenGenerator.Generate()
            };

            await _tokenRepository.SaveNewRefreshToken(newRefreshToken);
            await _unitOfWork.Commit();

            return new ResponseTokensJson
            {
                AccessToken = _accessTokenGenerator.Generate(refreshToken.User.UserIdentifier),
                RefreshToken = newRefreshToken.Value,
            };
        }
    }
}
