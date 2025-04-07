using Bogus;
using CommonTestUtilities.Requests;
using FluentAssertions;
using Forum.Application.UseCases.Comment;
using Forum.Exceptions;

namespace Validators.Test.Comment
{
    public class CommentValidatorTest
    {
        [Fact]
        public void Success()
        {
            var request = RequestCommentJsonBuilder.Build();

            var validator = new CommentValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Content_Empty()
        {
            var request = RequestCommentJsonBuilder.Build();
            request.Content = string.Empty;

            var validator = new CommentValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().Which.ErrorMessage.Should().Be(ResourceMessagesException.CONTENT_EMPTY);
        }

        [Fact]
        public void Error_Content_Max_Length()
        {
            var request = RequestCommentJsonBuilder.Build();
            request.Content = new Faker().Lorem.Letter(2001);

            var validator = new CommentValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().Which.ErrorMessage.Should().Be(ResourceMessagesException.CONTENT_MAX_LENGTH);
        }
    }
}
