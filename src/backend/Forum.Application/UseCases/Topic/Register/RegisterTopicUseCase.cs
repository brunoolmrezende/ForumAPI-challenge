using AutoMapper;
using Forum.Communication.Request;
using Forum.Communication.Response;
using Forum.Domain.Repository;
using Forum.Domain.Repository.Topic;
using Forum.Domain.Services;
using Forum.Exceptions.ExceptionBase;

namespace Forum.Application.UseCases.Topic.Register
{
    public class RegisterTopicUseCase(
        ILoggedUser loggedUser, 
        IMapper mapper,
        ITopicWriteOnlyRepository writeOnlyRepository,
        IUnitOfWork unitOfWork) : IRegisterTopicUseCase
    {
        private readonly ILoggedUser _loggedUser = loggedUser;
        private readonly IMapper _mapper = mapper;
        private readonly ITopicWriteOnlyRepository _writeOnlyRepository = writeOnlyRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResponseRegisteredTopicJson> Execute(RequestRegisterTopicJson request)
        {
            Validate(request);

            var loggedUser = await _loggedUser.User();

            var topic = _mapper.Map<Domain.Entities.Topic>(request);
            topic.UserId = loggedUser.Id;

            await _writeOnlyRepository.Add(topic);
            await _unitOfWork.Commit();

            return _mapper.Map<ResponseRegisteredTopicJson>(topic);
        }

        private static void Validate(RequestRegisterTopicJson request)
        {
            var validator = new RegisterTopicValidator();

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errors);
            }       
        }
    }
}
