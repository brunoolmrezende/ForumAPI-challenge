using Forum.Communication.Request;
using Forum.Communication.Response;
using Forum.Domain.Entities;
using Forum.Domain.Repository;
using Forum.Domain.Repository.Token;
using Forum.Domain.Repository.User;
using Forum.Domain.Security.AccessToken;
using Forum.Domain.Security.Cryptography;
using Forum.Domain.Security.RefreshToken;
using Forum.Exceptions.ExceptionBase;

namespace Forum.Application.UseCases.Login.DoLogin
{
    public class DoLoginUseCase(
        IUserReadOnlyRepository readOnlyRepository,
        IPasswordEncryption encryption,
        IAccessTokenGenerator accessToken,
        IRefreshTokenGenerator refreshTokenGenerator,
        ITokenRepository tokenRepository,
        IUnitOfWork unitOfWork) : IDoLoginUseCase
    {
        private readonly IUserReadOnlyRepository _readOnlyRepository = readOnlyRepository;
        private readonly IPasswordEncryption _encryption = encryption;
        private readonly IAccessTokenGenerator _accessToken = accessToken;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator = refreshTokenGenerator;
        private readonly ITokenRepository _tokenRepository = tokenRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResponseRegisteredUserJson> Execute(RequestDoLoginJson request)
        {
            var user = await _readOnlyRepository.GetByEmail(request.Email);

            if (user is null || !_encryption.Verify(request.Password, user.Password))
            {
                throw new InvalidLoginException();
            }

            var refreshToken = await CreateAndSaveRefreshToken(user);

            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
                Tokens = new ResponseTokensJson
                {
                    AccessToken = _accessToken.Generate(user.UserIdentifier),
                    RefreshToken = refreshToken
                }
            };
        }

        private async Task<string> CreateAndSaveRefreshToken(Domain.Entities.User user)
        {
            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Value = _refreshTokenGenerator.Generate()
            };

            await _tokenRepository.SaveNewRefreshToken(refreshToken);
            await _unitOfWork.Commit();

            return refreshToken.Value;
        }
    }
}
