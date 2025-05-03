using Bogus;
using CommonTestUtilities.Requests;
using FluentAssertions;
using Forum.Application.UseCases.Auth.ResetPassword;
using Forum.Exceptions;

namespace Validators.Test.Auth.ResetPassword
{
    public class ResetPasswordValidatorTest
    {
        [Fact]
        public void Success()
        {
            var request = RequestResetPasswordJsonBuilder.Build(email: "email@teste.com", codeValue: "ABC123");

            var validator = new ResetPasswordValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Error_Email_Empty()
        {
            var request = RequestResetPasswordJsonBuilder.Build(email: string.Empty, codeValue: "ABC123");

            var validator = new ResetPasswordValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be(ResourceMessagesException.EMAIL_EMPTY);
        }

        [Fact]
        public void Error_Email_Invalid()
        {
            var request = RequestResetPasswordJsonBuilder.Build(email: "invalid email", codeValue: "ABC123");

            var validator = new ResetPasswordValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be(ResourceMessagesException.INVALID_EMAIL);
        }

        [Fact]
        public void Error_Email_Max_Length()
        {
            var request = RequestResetPasswordJsonBuilder.Build(email: $"{new string('a', 256)}@email.com", codeValue: "ABC123");

            var validator = new ResetPasswordValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be(ResourceMessagesException.EMAIL_MAX_LENGTH);
        }

        [Fact]
        public void Error_Password_Empty()
        {
            var request = RequestResetPasswordJsonBuilder.Build(email: "email@teste.com", codeValue: "ABC123");
            request.NewPassword = string.Empty;

            var validator = new ResetPasswordValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be(ResourceMessagesException.EMPTY_PASSWORD);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        public void Error_Password_Invalid(int passwordLenght)
        {
            var request = RequestResetPasswordJsonBuilder.Build(email: "email@teste.com", codeValue: "ABC123");
            request.NewPassword = new Faker().Internet.Password(length: passwordLenght);

            var validator = new ResetPasswordValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be(ResourceMessagesException.PASSWORD_LENGTH);
        }

        [Fact]
        public void Error_Empty_Code()
        {
            var request = RequestResetPasswordJsonBuilder.Build(email: "email@teste.com", codeValue: "ABC123");
            request.Code = string.Empty;

            var validator = new ResetPasswordValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be(ResourceMessagesException.EMPTY_CODE);
        }
    }
}
