using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using Forum.Application.UseCases.Auth.ResetPassword;
using Forum.Communication.Request;
using Forum.Domain.ValueObjects;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace UseCases.Test.Auth.ResetPassword
{
    public class ResetPasswordUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var code = ResetPasswordCodeBuilder.Build(user.Email);

            var request = RequestResetPasswordJsonBuilder.Build(user.Email, code.Value);

            var useCase = CreateUseCase(request, user, code);

            var result = await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Message.Should().Be(ResourceMessage.PASSWORD_CHANGED);
        }

        [Fact]
        public async Task Error_Code_Not_Found()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestResetPasswordJsonBuilder.Build(user.Email, "invalid-code");

            var useCase = CreateUseCase(request, user);

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().ThrowAsync<ResetPasswordCodeNotFoundException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.CODE_NOT_FOUND));
        }

        [Fact]
        public async Task Error_Code_Expired()
        {
            (var user, _) = UserBuilder.Build();

            var code = ResetPasswordCodeBuilder.Build(user.Email);
            code.CreatedOn = DateTime.UtcNow.AddHours(-1);

            var request = RequestResetPasswordJsonBuilder.Build(user.Email, code.Value);

            var useCase = CreateUseCase(request, user, code);

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().ThrowAsync<ResetPasswordCodeExpiredException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.EXPIRED_CODE));
        }

        [Fact]
        public async Task Error_User_Not_Found()
        {
            (var user, _) = UserBuilder.Build();

            var code = ResetPasswordCodeBuilder.Build(user.Email);

            var request = RequestResetPasswordJsonBuilder.Build(user.Email, code.Value);

            var useCase = CreateUseCase(request, null, code);

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().ThrowAsync<ResetPasswordCodeExpiredException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.EXPIRED_CODE));
        }

        private static ResetPasswordUseCase CreateUseCase(
            RequestResetPasswordJson request,
            Forum.Domain.Entities.User? user = null,
            Forum.Domain.Entities.ResetPasswordCode? resetPasswordCode = null)
        {
            var passwordCodeRepository = new ResetPasswordCodeRepositoryBuilder().GetCode(request, resetPasswordCode).Build();
            var userUpdateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetByEmail(user).Build();
            var encryption = PasswordEncryptionBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new ResetPasswordUseCase(
                passwordCodeRepository,
                userUpdateOnlyRepository,
                encryption,
                unitOfWork);
        }
    }
}
