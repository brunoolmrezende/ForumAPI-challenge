using AutoMapper;
using Forum.Communication.Request;
using Forum.Communication.Response;
using Forum.Domain.Repository;
using Forum.Domain.Repository.User;
using Forum.Domain.Security.Cryptography;

namespace Forum.Application.UseCases.User.Register
{
    public class RegisterUserUseCase(
        IMapper mapper,
        IUserWriteOnlyRepository userRepository,
        IPasswordEncryption encryption,
        IUnitOfWork unitOfWork) : IRegisterUserUseCase
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUserWriteOnlyRepository _userRepository = userRepository;
        private readonly IPasswordEncryption _encryption = encryption;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
        {
            Validate(request);

            var user = _mapper.Map<Domain.Entities.User>(request);

            user.Password = _encryption.Encrypt(user.Password);
            user.UserIdentifier = Guid.NewGuid();

            await _userRepository.Add(user);
            await _unitOfWork.Commit();

            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
            };
        }

        private void Validate(RequestRegisterUserJson request)
        {
            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                throw new Exception("Erro de validação");
            }
        }
    }
}
