using CommonTestUtilities.Requests;
using FluentAssertions;
using Forum.Application.UseCases.Topic.Register;
using Forum.Exceptions;

namespace Validators.Test.Topic
{
    public class TopicValidatorTest
    {
        [Fact]
        public void Success()
        {
            var request = RequestTopicJsonBuilder.Build();

            var validator = new TopicValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Title_Empty()
        {
            var request = RequestTopicJsonBuilder.Build();
            request.Title = string.Empty;

            var validator = new TopicValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().Which.ErrorMessage.Should().Be(ResourceMessagesException.TITLE_EMPTY);
        }

        [Fact]
        public void Error_Content_Empty()
        {
            var request = RequestTopicJsonBuilder.Build();
            request.Content = string.Empty;

            var validator = new TopicValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().Which.ErrorMessage.Should().Be(ResourceMessagesException.CONTENT_EMPTY);
        }
    }
}
