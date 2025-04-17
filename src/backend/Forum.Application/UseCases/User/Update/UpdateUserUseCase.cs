using System.Threading.Tasks;
using Forum.Communication.Request;
using Forum.Domain.Repository;
using Forum.Domain.Repository.User;
using Forum.Domain.Services;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace Forum.Application.UseCases.User.Update
{
    public class UpdateUserUseCase(
        ILoggedUser loggedUser, 
        IUserUpdateOnlyRepository userUpdateOnlyRepository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUnitOfWork unitOfWork) : IUpdateUserUseCase
    {
        private readonly ILoggedUser _loggedUser = loggedUser;
        private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository = userUpdateOnlyRepository;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository = userReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Execute(RequestUpdateUserJson request)
        {
            var loggedUser = await _loggedUser.User();

            await Validate(request, loggedUser.Email);

            var user = await _userUpdateOnlyRepository.GetById(loggedUser.Id);

            user.Name = request.Name;
            user.Email = request.Email;

            _userUpdateOnlyRepository.Update(user);
            await _unitOfWork.Commit();
        }

        private async Task Validate(RequestUpdateUserJson request, string currentEmail)
        {
            var validator = new UpdateUserValidator();

            var result = validator.Validate(request);

            var emailExists = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

            if  (emailExists && request.Email != currentEmail)
            {
                result.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ResourceMessagesException.EMAIL_ALREADY_REGISTERED));;
            }

            if (result.IsValid is false)
            {
                var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errors);
            }
        }
    }
}