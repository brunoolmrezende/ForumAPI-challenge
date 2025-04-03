using Forum.Communication.Request;
using Forum.Communication.Response;
using Forum.Domain.Repository.User;
using Forum.Domain.Security.Cryptography;
using Forum.Exceptions.ExceptionBase;

namespace Forum.Application.UseCases.Login.DoLogin
{
    public class DoLoginUseCase(
        IUserReadOnlyRepository readOnlyRepository,
        IPasswordEncryption encryption) : IDoLoginUseCase
    {
        private readonly IUserReadOnlyRepository _readOnlyRepository = readOnlyRepository;
        private readonly IPasswordEncryption _encryption = encryption;

        public async Task<ResponseRegisteredUserJson> Execute(RequestDoLoginJson request)
        {
            var user = await _readOnlyRepository.GetByEmail(request.Email);

            if (user is null || !_encryption.Verify(request.Password, user.Password))
            {
                throw new InvalidLoginException();
            }

            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
            };
        }
    }
}
