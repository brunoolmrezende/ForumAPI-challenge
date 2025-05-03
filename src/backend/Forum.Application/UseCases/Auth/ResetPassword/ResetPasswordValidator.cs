using FluentValidation;
using Forum.Communication.Request;
using Forum.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Application.UseCases.Auth.ResetPassword
{
    public class ResetPasswordValidator : AbstractValidator<RequestResetPasswordJson>
    {
        public ResetPasswordValidator()
        {
            RuleFor(user => user.Code)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.EMPTY_CODE);

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
