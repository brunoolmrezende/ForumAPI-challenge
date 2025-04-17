using Bogus;
using CommonTestUtilities.Requests;
using FluentAssertions;
using Forum.Application.UseCases.User.Register;
using Forum.Exceptions;

namespace Validators.Test.User.Update
{
    public class UpdateUserValidatorTest
    {
        [Fact]
        public void Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Name_Empty()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().Which.ErrorMessage.Should().Be(ResourceMessagesException.NAME_EMPTY);
        }

        [Fact]
        public void Error_Email_Empty()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Email = string.Empty;

            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().Which.ErrorMessage.Should().Be(ResourceMessagesException.EMAIL_EMPTY);
        }

        [Fact]
        public void Error_Email_Invalid()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Email = "invalid email";

            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().Which.ErrorMessage.Should().Be(ResourceMessagesException.INVALID_EMAIL);
        }

        [Fact]
        public void Error_Name_Max_Length()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = new string('a', 256);

            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().Which.ErrorMessage.Should().Be(ResourceMessagesException.NAME_MAX_LENGTH);
        }

        [Fact]
        public void Error_Email_Max_Length()
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Email = $"{new Faker().Lorem.Letter(256)}@teste.com";

            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().Which.ErrorMessage.Should().Be(ResourceMessagesException.EMAIL_MAX_LENGTH);
        }
    }
}
