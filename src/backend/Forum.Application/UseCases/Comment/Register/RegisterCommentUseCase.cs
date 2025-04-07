using AutoMapper;
using Forum.Communication.Request;
using Forum.Communication.Response;
using Forum.Domain.Repository;
using Forum.Domain.Repository.Comment;
using Forum.Domain.Repository.Topic;
using Forum.Domain.Services;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace Forum.Application.UseCases.Comment.Register
{
    public class RegisterCommentUseCase(
        ILoggedUser loggedUser,
        ITopicReadOnlyRepository topicReadOnlyRepository,
        ICommentWriteOnlyRepository commentWriteOnlyRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IRegisterCommentUseCase
    {
        private readonly ILoggedUser _loggedUser = loggedUser;
        private readonly ITopicReadOnlyRepository _topicReadOnlyRepository = topicReadOnlyRepository;
        private readonly ICommentWriteOnlyRepository _commentWriteOnlyRepository = commentWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseRegisteredCommentJson> Execute(long topicId, RequestCommentJson request)
        {
            Validate(request);

            var topicExists = await _topicReadOnlyRepository.ExistsById(topicId);

            if (topicExists is false)
            {
                throw new NotFoundException(ResourceMessagesException.TOPIC_NOT_FOUND);
            }

            var loggedUser = await _loggedUser.User();

            var comment = new Domain.Entities.Comment
            {
                Content = request.Content,
                TopicId = topicId,
                UserId = loggedUser.Id
            };

            await _commentWriteOnlyRepository.Add(comment);
            await _unitOfWork.Commit();

            return _mapper.Map<ResponseRegisteredCommentJson>(comment);
        }

        private static void Validate(RequestCommentJson request)
        {
            var validator = new CommentValidator();

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
