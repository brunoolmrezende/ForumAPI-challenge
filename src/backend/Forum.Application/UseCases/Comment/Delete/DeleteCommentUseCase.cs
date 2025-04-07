
using Forum.Domain.Repository;
using Forum.Domain.Repository.Comment;
using Forum.Domain.Repository.Topic;
using Forum.Domain.Services;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace Forum.Application.UseCases.Comment.Delete
{
    public class DeleteCommentUseCase(
        ITopicReadOnlyRepository topicReadOnlyRepository,
        ICommentUpdateOnlyRepository commentUpdateOnlyRepository,
        ICommentWriteOnlyRepository commentWriteOnlyRepository,
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork) : IDeleteCommentUseCase
    {
        private readonly ITopicReadOnlyRepository _topicReadOnlyRepository = topicReadOnlyRepository;
        private readonly ICommentUpdateOnlyRepository _commentUpdateOnlyRepository = commentUpdateOnlyRepository;
        private readonly ICommentWriteOnlyRepository _commentWriteOnlyRepository = commentWriteOnlyRepository;
        private readonly ILoggedUser _loggedUser = loggedUser;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Execute(long topicId, long commentId)
        {
            var topixExists = await _topicReadOnlyRepository.ExistsById(topicId);

            if (topixExists is false)
            {
                throw new NotFoundException(ResourceMessagesException.TOPIC_NOT_FOUND);
            }

            var loggedUser = await _loggedUser.User();

            var comment = await _commentUpdateOnlyRepository.GetById(commentId, loggedUser.Id, topicId);

            if (comment is null)
            {
                throw new NotFoundException(ResourceMessagesException.COMMENT_NOT_FOUND);
            }

            _commentWriteOnlyRepository.Delete(comment);
            await _unitOfWork.Commit();
        }
    }
}
