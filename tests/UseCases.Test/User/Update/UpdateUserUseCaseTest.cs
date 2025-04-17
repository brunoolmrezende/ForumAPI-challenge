using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using Forum.Application.UseCases.User.Update;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace UseCases.Test.User.Update
{
    public class UpdateUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _ ) = UserBuilder.Build();

            var request = RequestUpdateUserJsonBuilder.Build();

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().NotThrowAsync();
            
            user.Name.Should().Be(request.Name);
            user.Email.Should().Be(request.Email);
        }

        [Fact]
        public async Task Error_Email_Already_Registrered()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestUpdateUserJsonBuilder.Build();

            var useCase = CreateUseCase(user, request.Email);

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
        }

        [Fact]
        public async Task Error_Name_Empty()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = string.Empty;

            var useCase = CreateUseCase(user);

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().ThrowAsync<ErrorOnValidationException>()
                .Where(error => error.GetErrorMessage().Count == 1
                    && error.GetErrorMessage().Contains(ResourceMessagesException.NAME_EMPTY));
        }

        private static UpdateUserUseCase CreateUseCase(Forum.Domain.Entities.User user, string? email = null)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder().ExistActiveUserWithEmail(email).Build();
            var userUpdateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            return new UpdateUserUseCase(loggedUser, userUpdateOnlyRepository, userReadOnlyRepository, unitOfWork);
        }
    }
}
