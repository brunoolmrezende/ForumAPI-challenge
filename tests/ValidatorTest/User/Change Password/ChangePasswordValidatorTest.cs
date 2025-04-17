using CommonTestUtilities.Requests;
using FluentAssertions;
using Forum.Application.UseCases.User.Change_Password;
using Forum.Exceptions;

namespace Validators.Test.User.Change_Password
{
    public class ChangePasswordValidatorTest
    {
        [Fact]
        public void Success()
        {
            var request = RequestChangePasswordJsonBuilder.Build();

            var validator = new ChangePasswordValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Empty_Password()
        {
            var request = RequestChangePasswordJsonBuilder.Build(0);

            var validator = new ChangePasswordValidator();

            var result = validator.Validate(request); 

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error.ErrorMessage.Equals(ResourceMessagesException.EMPTY_PASSWORD));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        public void Error_Password_Length(int passwordLength)
        {
            var request = RequestChangePasswordJsonBuilder.Build(passwordLength);

            var validator = new ChangePasswordValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error.ErrorMessage.Equals(ResourceMessagesException.PASSWORD_LENGTH));
        }
    }
}
