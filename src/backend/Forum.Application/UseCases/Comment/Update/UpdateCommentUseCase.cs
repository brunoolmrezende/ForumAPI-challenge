using Forum.Communication.Request;
using Forum.Domain.Repository;
using Forum.Domain.Repository.Comment;
using Forum.Domain.Services;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace Forum.Application.UseCases.Comment.Update
{
    public class UpdateCommentUseCase(
        ICommentUpdateOnlyRepository commentUpdateOnlyRepository,
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork) : IUpdateCommentUseCase
    {
        private readonly ICommentUpdateOnlyRepository _commentUpdateOnlyRepository = commentUpdateOnlyRepository;
        private readonly ILoggedUser _loggedUser = loggedUser;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Execute(long commentId, RequestCommentJson request)
        {
            Validate(request);

            var loggedUser = await _loggedUser.User();

            var comment = await _commentUpdateOnlyRepository.GetById(commentId, loggedUser.Id);

            if (comment is null)
            {
                throw new NotFoundException(ResourceMessagesException.COMMENT_NOT_FOUND);
            }

            comment.Content = request.Content;

            _commentUpdateOnlyRepository.Update(comment);
            await _unitOfWork.Commit();
        }

        private static void Validate(RequestCommentJson request)
        {
            var validator = new CommentValidator();

            var result = validator.Validate(request);

            if (result.IsValid is false)
            {
                var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
