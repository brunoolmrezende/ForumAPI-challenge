using AutoMapper;
using Forum.Application.UseCases.Topic.Register;
using Forum.Communication.Request;
using Forum.Domain.Repository;
using Forum.Domain.Repository.Topic;
using Forum.Domain.Services;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace Forum.Application.UseCases.Topic.Update
{
    public class UpdateTopicUseCase(
        ILoggedUser loggedUser,
        ITopicUpdateOnlyRepository updateOnlyRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork) : IUpdateTopicUseCase
    {
        private readonly ILoggedUser _loggedUser = loggedUser;
        private readonly ITopicUpdateOnlyRepository _updateOnlyRepository = updateOnlyRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Execute(RequestTopicJson request, long id)
        {
            Validate(request);

            var loggedUser = await _loggedUser.User();

            var topic = await _updateOnlyRepository.GetById(id, loggedUser.Id);

            if (topic is null)
            {
                throw new NotFoundException(ResourceMessagesException.TOPIC_NOT_FOUND);
            }

            _mapper.Map(request, topic);

            _updateOnlyRepository.Update(topic);
            await _unitOfWork.Commit();
        }

        private static void Validate(RequestTopicJson request)
        {
            var validator = new TopicValidator();

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
