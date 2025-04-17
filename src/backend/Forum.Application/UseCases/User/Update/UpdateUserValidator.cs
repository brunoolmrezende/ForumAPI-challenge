using FluentValidation;
using Forum.Communication.Request;
using Forum.Exceptions;

namespace Forum.Application.UseCases.User.Update
{
    public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
    {
        public UpdateUserValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.NAME_EMPTY)
                .MaximumLength(255)
                .WithMessage(ResourceMessagesException.NAME_MAX_LENGTH);


            RuleFor(user => user.Email)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.EMAIL_EMPTY)
                .MaximumLength(255)
                .WithMessage(ResourceMessagesException.EMAIL_MAX_LENGTH);

            When(user => !string.IsNullOrWhiteSpace(user.Email), () =>
            {
                RuleFor(user => user.Email)
                 .EmailAddress()
                 .WithMessage(ResourceMessagesException.INVALID_EMAIL);
            });
        }
    }
}