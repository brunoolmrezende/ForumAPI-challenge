using FluentValidation;
using Forum.Communication.Request;
using Forum.Exceptions;

namespace Forum.Application.UseCases.Comment
{
    public class CommentValidator : AbstractValidator<RequestCommentJson>
    {
        public CommentValidator()
        {
            RuleFor(comment => comment.Content)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.CONTENT_EMPTY)
                .MaximumLength(2000)
                .WithMessage(ResourceMessagesException.CONTENT_MAX_LENGTH);
        }
    }
}
