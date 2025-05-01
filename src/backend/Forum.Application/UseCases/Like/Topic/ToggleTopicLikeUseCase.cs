using Forum.Domain.Entities;
using Forum.Domain.Repository;
using Forum.Domain.Repository.Like.TopicLike;
using Forum.Domain.Repository.Topic;
using Forum.Domain.Services;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace Forum.Application.UseCases.Like.Topic
{
    public class ToggleTopicLikeUseCase(
        ILoggedUser loggedUser,
        ITopicReadOnlyRepository topicReadOnlyRepository,
        ITopicLikeUpdateOnlyRepository topicLikeUpdateOnlyRepository,
        ITopicLikeWriteOnlyRepository topicLikeWriteOnlyRepository,
        IUnitOfWork unitOfWork) : IToggleTopicLikeUseCase
    {
        private readonly ILoggedUser _loggedUser = loggedUser;
        private readonly ITopicReadOnlyRepository _topicReadOnlyRepository = topicReadOnlyRepository;
        private readonly ITopicLikeUpdateOnlyRepository _topicLikeUpdateOnlyRepository = topicLikeUpdateOnlyRepository;
        private readonly ITopicLikeWriteOnlyRepository _topicLikeWriteOnlyRepository = topicLikeWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Execute(long topicId)
        {
            var loggedUser = await _loggedUser.User();

            var topicExists = await _topicReadOnlyRepository.ExistsById(topicId);

            if (topicExists is false)
            {
                throw new NotFoundException(ResourceMessagesException.TOPIC_NOT_FOUND);
            }

            var topicLike = await _topicLikeUpdateOnlyRepository.GetById(loggedUser.Id, topicId);

            if (topicLike is null)
            {
                await _topicLikeWriteOnlyRepository.Add(new TopicLike
                {
                    TopicId = topicId,
                    UserId = loggedUser.Id,
                });
            }
            else
            {
                _topicLikeWriteOnlyRepository.Delete(topicLike);
            }

            await _unitOfWork.Commit();
        }
    }
}
