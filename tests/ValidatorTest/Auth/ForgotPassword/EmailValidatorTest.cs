using CommonTestUtilities.Entities;
using CommonTestUtilities.Requests;
using FluentAssertions;
using Forum.Exceptions;

namespace Validators.Test.Auth.ForgotPassword
{
    public class EmailValidatorTest
    {
        [Fact]
        public void Success()
        {
            (var user, _ ) = UserBuilder.Build();

            var request = RequestForgotPasswordJsonBuilder.Build(user.Email);

            var validator = new Forum.Application.UseCases.Auth.ForgotPassword.EmailValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Email_Empty()
        {
            var request = RequestForgotPasswordJsonBuilder.Build(string.Empty);

            var validator = new Forum.Application.UseCases.Auth.ForgotPassword.EmailValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be(ResourceMessagesException.EMAIL_EMPTY);
        }

        [Fact]
        public void Error_Email_Invalid()
        {
            var request = RequestForgotPasswordJsonBuilder.Build("invalid-email");

            var validator = new Forum.Application.UseCases.Auth.ForgotPassword.EmailValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be(ResourceMessagesException.INVALID_EMAIL);
        }
    }
}
