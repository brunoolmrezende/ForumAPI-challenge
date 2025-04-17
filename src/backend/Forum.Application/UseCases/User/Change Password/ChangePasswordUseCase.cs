using FluentValidation.Results;
using Forum.Communication.Request;
using Forum.Domain.Repository;
using Forum.Domain.Repository.User;
using Forum.Domain.Security.Cryptography;
using Forum.Domain.Services;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace Forum.Application.UseCases.User.Change_Password
{
    public class ChangePasswordUseCase(
        ILoggedUser loggedUser,
        IPasswordEncryption encryption,
        IUserUpdateOnlyRepository updateOnlyRepository,
        IUnitOfWork unitOfWork) : IChangePasswordUseCase
    {
        private readonly ILoggedUser _loggedUser = loggedUser;
        private readonly IPasswordEncryption _encryption = encryption;
        private readonly IUserUpdateOnlyRepository _updateOnlyRepository = updateOnlyRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Execute(RequestChangePasswordJson request)
        {
            var loggedUser = await _loggedUser.User();

            Validate(request, loggedUser);

            var user = await _updateOnlyRepository.GetById(loggedUser.Id);
            user.Password = _encryption.Encrypt(request.NewPassword);
            
            _updateOnlyRepository.Update(user);
            await _unitOfWork.Commit();
        }

        private void Validate(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
        {
            var validator = new ChangePasswordValidator();

            var result = validator.Validate(request);

            if (_encryption.Verify(request.OldPassword, loggedUser.Password) is false)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
            }

            if (result.IsValid is false)
            {
                var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
