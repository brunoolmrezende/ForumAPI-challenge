using Forum.Domain.Entities;
using Forum.Domain.Repository;
using Forum.Domain.Repository.Comment;
using Forum.Domain.Repository.Like.CommentLike;
using Forum.Domain.Services;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace Forum.Application.UseCases.Like.Comment
{
    public class ToggleCommentLikeUseCase(
        ILoggedUser loggedUser,
        ICommentReadOnlyRepository commentReadOnlyRepository,
        ICommentLikeUpdateOnlyRepository commentLikeUpdateOnlyRepository,
        ICommentLikeWriteOnlyRepository commentLikeWriteOnlyRepository,
        IUnitOfWork unitOfWork) : IToggleCommentLikeUseCase
    {
        private readonly ILoggedUser _loggedUser = loggedUser;
        private readonly ICommentReadOnlyRepository _commentReadOnlyRepository = commentReadOnlyRepository;
        private readonly ICommentLikeUpdateOnlyRepository _commentLikeUpdateOnlyRepository = commentLikeUpdateOnlyRepository;
        private readonly ICommentLikeWriteOnlyRepository _commentLikeWriteOnlyRepository = commentLikeWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Execute(long commentId)
        {
            var loggedUser = await _loggedUser.User();

            var commentExists = await _commentReadOnlyRepository.ExistsById(commentId);

            if (commentExists is false)
            {
                throw new NotFoundException(ResourceMessagesException.COMMENT_NOT_FOUND);
            }

            var commentLike = await _commentLikeUpdateOnlyRepository.GetById(loggedUser.Id, commentId);

            if (commentLike is null)
            {
                await _commentLikeWriteOnlyRepository.Add(new CommentLike
                {
                    CommentId = commentId,
                    UserId = loggedUser.Id,
                });
            }
            else
            {
                _commentLikeWriteOnlyRepository.Delete(commentLike);
            }

            await _unitOfWork.Commit();
        }
    }
}
