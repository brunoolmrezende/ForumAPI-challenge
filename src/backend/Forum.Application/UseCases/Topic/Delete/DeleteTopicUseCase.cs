
using Forum.Domain.Repository;
using Forum.Domain.Repository.Topic;
using Forum.Domain.Services;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace Forum.Application.UseCases.Topic.Delete
{
    public class DeleteTopicUseCase(
        ILoggedUser loggedUser,
        ITopicReadOnlyRepository readOnlyRepository,
        ITopicWriteOnlyRepository writeOnlyRepository,
        IUnitOfWork unitOfWork) : IDeleteTopicUseCase
    {
        private readonly ILoggedUser _loggedUser = loggedUser;
        private readonly ITopicReadOnlyRepository _readOnlyRepository = readOnlyRepository;
        private readonly ITopicWriteOnlyRepository _writeOnlyRepository = writeOnlyRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Execute(long id)
        {
            var loggedUser = await _loggedUser.User();

            var topic = await _readOnlyRepository.GetById(id, loggedUser.Id);

            if (topic is null)
            {
                throw new NotFoundException(ResourceMessagesException.TOPIC_NOT_FOUND);
            }

            await _writeOnlyRepository.Delete(topic.Id);
            await _unitOfWork.Commit();
        }
    }
}
