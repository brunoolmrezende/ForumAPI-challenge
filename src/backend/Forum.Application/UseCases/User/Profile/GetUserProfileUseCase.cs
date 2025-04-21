using AutoMapper;
using Forum.Communication.Response;
using Forum.Domain.Repository.User;
using Forum.Domain.Services;

namespace Forum.Application.UseCases.User.Profile
{
    public class GetUserProfileUseCase(
        ILoggedUser loggedUser,
        IUserReadOnlyRepository repository,
        IMapper mapper) : IGetUserProfileUseCase
    {
        private readonly ILoggedUser _loggedUser = loggedUser;
        private readonly IUserReadOnlyRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseUserProfileJson> Execute()
        {
            var loggedUser = await _loggedUser.User();

            var user = await _repository.GetProfile(loggedUser.Id);

            return _mapper.Map<ResponseUserProfileJson>(user);
        }
    }
}
