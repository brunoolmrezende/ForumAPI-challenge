using Forum.Communication.Request;
using Forum.Communication.Response;
using Forum.Domain.Entities;
using Forum.Domain.Repository;
using Forum.Domain.Repository.ResetPasswordCode;
using Forum.Domain.Repository.User;
using Forum.Domain.Security.ResetPasswordCode;
using Forum.Domain.Services;
using Forum.Domain.ValueObjects;
using Forum.Exceptions.ExceptionBase;

namespace Forum.Application.UseCases.Auth.ForgotPassword
{
    public class ForgotPasswordUseCase(
        IUserReadOnlyRepository userReadOnlyRepository,
        IResetPasswordCodeGenerator codeGenerator,
        IResetPasswordCodeRepository passwordCodeRepository,
        IUnitOfWork unitOfWork,
        IResetPasswordCodeSendEmail mail) : IForgotPasswordUseCase
    {
        private readonly IUserReadOnlyRepository _userReadOnlyRepository = userReadOnlyRepository;
        private readonly IResetPasswordCodeGenerator _codeGenerator = codeGenerator;
        private readonly IResetPasswordCodeRepository _passwordCodeRepository = passwordCodeRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IResetPasswordCodeSendEmail _mail = mail;

        public async Task<ResponseMessageJson> Execute(RequestForgotPasswordJson request)
        {
            Validate(request);

            var existsUserWithEmail = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

            if (existsUserWithEmail)
            {
                var randomCode = _codeGenerator.Generate();

                var resetCodeEntity = new ResetPasswordCode
                {
                    UserEmail = request.Email,
                    Value = randomCode,
                };

                await _passwordCodeRepository.SaveNewCode(resetCodeEntity);
                await _unitOfWork.Commit();

                await _mail.SendEmail(
                    request.Email, 
                    subject: ResourceMessage.SUBJECT_CONTENT, 
                    body: $"{ResourceMessage.BODY_CONTENT}: {randomCode}"); 
            }

            return new ResponseMessageJson(ResourceMessage.GENERIC_MESSAGE);
        }

        private static void Validate(RequestForgotPasswordJson request)
        {
            var validator = new EmailValidator();

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
