using Forum.Communication.Request;
using Forum.Communication.Response;
using Forum.Domain.Repository;
using Forum.Domain.Repository.ResetPasswordCode;
using Forum.Domain.Repository.User;
using Forum.Domain.Security.Cryptography;
using Forum.Domain.ValueObjects;
using Forum.Exceptions.ExceptionBase;

namespace Forum.Application.UseCases.Auth.ResetPassword
{
    public class ResetPasswordUseCase(
        IResetPasswordCodeRepository passwordCodeRepository,
        IUserUpdateOnlyRepository userUpdateOnlyRepository,
        IPasswordEncryption encryption,
        IUnitOfWork unitOfWork) : IResetPasswordUseCase
    {
        private readonly IResetPasswordCodeRepository _passwordCodeRepository = passwordCodeRepository;
        private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository = userUpdateOnlyRepository;
        private readonly IPasswordEncryption _encryption = encryption;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResponseMessageJson> Execute(RequestResetPasswordJson request)
        {
            Validate(request);

            var code = await _passwordCodeRepository.GetCode(request.Code, request.Email);

            if (code is null)
            {
                throw new ResetPasswordCodeNotFoundException();
            }

            var codeExpirationTime = code.CreatedOn.AddMinutes(ForumRuleConstants.MAXIMUM_RESET_PASSWORD_CODE_TIME_IN_MINUTES);

            var user = await _userUpdateOnlyRepository.GetByEmail(request.Email);

            if ((codeExpirationTime < DateTime.UtcNow) || user is null)
            {
                throw new ResetPasswordCodeExpiredException();
            }

            user.Password = _encryption.Encrypt(request.NewPassword);
            
            _userUpdateOnlyRepository.Update(user);
            await _unitOfWork.Commit();

            return new ResponseMessageJson(ResourceMessage.PASSWORD_CHANGED);
        }

        private static void Validate(RequestResetPasswordJson request)
        {
            var validator = new ResetPasswordValidator();

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
