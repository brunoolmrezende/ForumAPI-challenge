using AutoMapper;
using FluentValidation.Results;
using Forum.Communication.Request;
using Forum.Communication.Response;
using Forum.Domain.Repository;
using Forum.Domain.Repository.User;
using Forum.Domain.Security.Cryptography;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace Forum.Application.UseCases.User.Register
{
    public class RegisterUserUseCase(
        IMapper mapper,
        IUserWriteOnlyRepository userWriteOnlyRepository,
        IPasswordEncryption encryption,
        IUnitOfWork unitOfWork,
        IUserReadOnlyRepository userReadOnlyRepository) : IRegisterUserUseCase
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUserWriteOnlyRepository _userWriteOnlyRepository = userWriteOnlyRepository;
        private readonly IPasswordEncryption _encryption = encryption;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository = userReadOnlyRepository;

        public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
        {
            await Validate(request);

            var user = _mapper.Map<Domain.Entities.User>(request);

            user.Password = _encryption.Encrypt(request.Password);
            user.UserIdentifier = Guid.NewGuid();

            await _userWriteOnlyRepository.Add(user);
            await _unitOfWork.Commit();

            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
            };
        }

        private async Task Validate(RequestRegisterUserJson request)
        {
            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            var emailAlreadyRegistered = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);

            if (emailAlreadyRegistered)
            {
                result.Errors.Add(new ValidationFailure("error already registered", ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
            }

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
