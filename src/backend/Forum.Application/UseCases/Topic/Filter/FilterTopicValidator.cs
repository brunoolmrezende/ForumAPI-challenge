using FluentValidation;
using Forum.Communication.Request;
using Forum.Exceptions;

namespace Forum.Application.UseCases.Topic.Filter
{
    public class FilterTopicValidator : AbstractValidator<RequestFilterTopicJson>
    {
        public FilterTopicValidator()
        {
            RuleFor(topic => topic.Title)
                .MaximumLength(255).WithMessage(ResourceMessagesException.TITLE_MAX_LENGTH);

            RuleFor(topic => topic.Content)
                .MaximumLength(2000).WithMessage(ResourceMessagesException.CONTENT_MAX_LENGTH);
        }
    }
}
