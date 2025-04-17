using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using Forum.Application.UseCases.User.Change_Password;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace UseCases.Test.User.Change_Password
{
    public class ChangePasswordUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, var password ) = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();
            request.OldPassword = password;

            var useCase = CreateUseCase(user);

            Func<Task> act = async() => await useCase.Execute(request);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Error_Invalid_Old_Password()
        {
            (var user, var password) = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
        }

        private ChangePasswordUseCase CreateUseCase(Forum.Domain.Entities.User user)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var encryption = PasswordEncryptionBuilder.Build();
            var updateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new ChangePasswordUseCase(loggedUser, encryption, updateOnlyRepository, unitOfWork);
        }
    }
}
