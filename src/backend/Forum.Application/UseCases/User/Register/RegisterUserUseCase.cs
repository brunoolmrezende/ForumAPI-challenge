using AutoMapper;
using FluentValidation.Results;
using Forum.Communication.Request;
using Forum.Communication.Response;
using Forum.Domain.Entities;
using Forum.Domain.Repository;
using Forum.Domain.Repository.Token;
using Forum.Domain.Repository.User;
using Forum.Domain.Security.AccessToken;
using Forum.Domain.Security.Cryptography;
using Forum.Domain.Security.RefreshToken;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace Forum.Application.UseCases.User.Register
{
    public class RegisterUserUseCase(
        IMapper mapper,
        IUserWriteOnlyRepository userWriteOnlyRepository,
        IPasswordEncryption encryption,
        IUnitOfWork unitOfWork,
        IUserReadOnlyRepository userReadOnlyRepository,
        IAccessTokenGenerator accessToken,
        IRefreshTokenGenerator refreshTokenGenerator,
        ITokenRepository tokenRepository) : IRegisterUserUseCase
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUserWriteOnlyRepository _userWriteOnlyRepository = userWriteOnlyRepository;
        private readonly IPasswordEncryption _encryption = encryption;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository = userReadOnlyRepository;
        private readonly IAccessTokenGenerator _accessToken = accessToken;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator = refreshTokenGenerator;
        private readonly ITokenRepository _tokenRepository = tokenRepository;

        public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
        {
            await Validate(request);

            var user = _mapper.Map<Domain.Entities.User>(request);

            user.Password = _encryption.Encrypt(request.Password);
            user.UserIdentifier = Guid.NewGuid();

            await _userWriteOnlyRepository.Add(user);
            await _unitOfWork.Commit();

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

        private async Task Validate(RequestRegisterUserJson request)
        {
            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            var emailAlreadyRegistered = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

            if (emailAlreadyRegistered)
            {
                result.Errors.Add(new ValidationFailure("error already registered", ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
            }

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
