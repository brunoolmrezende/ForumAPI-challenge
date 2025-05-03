using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services.Email;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using Forum.Application.UseCases.Auth.ForgotPassword;
using Forum.Domain.ValueObjects;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace UseCases.Test.Auth.ForgotPassword
{
    public class ForgotPasswordUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestForgotPasswordJsonBuilder.Build(user.Email);

            var useCase = CreateUseCase(user.Email);

            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Message.Should().Be(ResourceMessage.GENERIC_MESSAGE);
        }

        [Fact]
        public async Task Error_Email_Empty()
        {
            var request = RequestForgotPasswordJsonBuilder.Build(string.Empty);

            var useCase = CreateUseCase();

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.EMAIL_EMPTY));
        }

        [Fact]
        public async Task Error_Email_Invalid()
        {
            var request = RequestForgotPasswordJsonBuilder.Build("invalid-email");

            var useCase = CreateUseCase();

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.INVALID_EMAIL));
        }

        private static ForgotPasswordUseCase CreateUseCase(string? userEmail = null)
        {
            var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder().ExistActiveUserWithEmail(userEmail).Build();
            var codeGenerator = ResetPasswordCodeGeneratorBuilder.Build();
            var passwordCodeRepository= new ResetPasswordCodeRepositoryBuilder().Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var mail = ResetPasswordCodeSendEmailBuilder.Build();

            return new ForgotPasswordUseCase(
                userReadOnlyRepository,
                codeGenerator,
                passwordCodeRepository,
                unitOfWork,
                mail);
        }
    }
}
