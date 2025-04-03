using FluentValidation;
using Forum.Communication.Request;
using Forum.Exceptions;

namespace Forum.Application.UseCases.User.Register
{
    public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
    {
        public RegisterUserValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.NAME_EMPTY);


            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.EMAIL_EMPTY);

            When(user => !string.IsNullOrWhiteSpace(user.Email), () =>
            {
                RuleFor(user => user.Email)
                 .EmailAddress()
                 .WithMessage(ResourceMessagesException.INVALID_EMAIL);
            });


            RuleFor(user => user.Password)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.EMPTY_PASSWORD);

            When(user => !string.IsNullOrWhiteSpace(user.Password), () =>
            {
                RuleFor(user => user.Password)
                    .MinimumLength(8)
                    .WithMessage(ResourceMessagesException.PASSWORD_LENGTH);
            });
        }
    }
}
