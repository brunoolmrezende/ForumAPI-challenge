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

            if (loggedUser is not null)
            {
                topicDetails.LikedByCurrentUser = topic.Likes.Any(like => like.UserId.Equals(loggedUser.Id));

                var commentsById = topic.Comments.ToDictionary(c => c.Id, c => c);

                foreach (var comment in topicDetails.Comments)
                {
                    if (commentsById.TryGetValue(comment.Id, out var commentEntity))
                    {
                        comment.LikedByCurrentUser = commentEntity.Likes.Any(like => like.UserId.Equals(loggedUser.Id));
                    }
                }
            }

            return topicDetails;
        }
    }
}
