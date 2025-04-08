using AutoMapper;
using Forum.Communication.Response;
using Forum.Domain.Repository.Topic;
using Forum.Domain.Services;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace Forum.Application.UseCases.Topic.GetById
{
    public class GetTopicByIdUseCase(
        ITopicReadOnlyRepository readOnlyRepository,
        ILoggedUser loggedUser,
        IMapper mapper) : IGetTopicByIdUseCase
    {
        private readonly ITopicReadOnlyRepository _readOnlyRepository = readOnlyRepository;
        private readonly ILoggedUser _loggedUser = loggedUser;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseTopicDetailsJson> Execute(long id)
        {
            var topic = await _readOnlyRepository.GetTopicDetails(id);

            if (topic is null)
            {
                throw new NotFoundException(ResourceMessagesException.TOPIC_NOT_FOUND);
            }

            var topicDetails = _mapper.Map<ResponseTopicDetailsJson>(topic);

            var loggedUser = await _loggedUser.TryGetUser();

            topicDetails.LikedByCurrentUser = loggedUser is not null
                    && topic.Likes.Any(like => like.UserId.Equals(loggedUser.Id));

            return topicDetails;
        }
    }
}
