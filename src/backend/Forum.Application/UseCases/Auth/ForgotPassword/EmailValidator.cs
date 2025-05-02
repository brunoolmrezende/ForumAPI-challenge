using FluentValidation;
using Forum.Communication.Request;
using Forum.Exceptions;

namespace Forum.Application.UseCases.Auth.ForgotPassword
{
    public class EmailValidator : AbstractValidator<RequestForgotPasswordJson>
    {
        public EmailValidator()
        {
            RuleFor(request => request.Email)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.EMAIL_EMPTY);

            When(request => !string.IsNullOrWhiteSpace(request.Email), () =>
            {
                RuleFor(request => request.Email)
                    .EmailAddress()
                    .WithMessage(ResourceMessagesException.INVALID_EMAIL);
            });
        }
    }
}
