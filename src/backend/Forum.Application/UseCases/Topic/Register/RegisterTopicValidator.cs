using FluentValidation;
using Forum.Communication.Request;
using Forum.Exceptions;

namespace Forum.Application.UseCases.Topic.Register
{
    public class RegisterTopicValidator : AbstractValidator<RequestRegisterTopicJson>
    {
        public RegisterTopicValidator()
        {
            RuleFor(topic => topic.Title)
                .NotEmpty().WithMessage(ResourceMessagesException.TITLE_EMPTY)
                .MaximumLength(255).WithMessage(ResourceMessagesException.TITLE_MAX_LENGTH);

            RuleFor(topic => topic.Content)
                .NotEmpty().WithMessage(ResourceMessagesException.CONTENT_EMPTY)
                .MaximumLength(2000).WithMessage(ResourceMessagesException.CONTENT_MAX_LENGTH);
        }
    }
}
