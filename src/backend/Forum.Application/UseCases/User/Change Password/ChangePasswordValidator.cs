using FluentValidation;
using Forum.Communication.Request;
using Forum.Exceptions;

namespace Forum.Application.UseCases.User.Change_Password
{
    public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
    {
        public ChangePasswordValidator()
        {
            RuleFor(user => user.NewPassword)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.EMPTY_PASSWORD);

            When(user => !string.IsNullOrWhiteSpace(user.NewPassword), () =>
            {
                RuleFor(user => user.NewPassword)
                    .MinimumLength(8)
                    .WithMessage(ResourceMessagesException.PASSWORD_LENGTH);
            });
        }
    }
}
