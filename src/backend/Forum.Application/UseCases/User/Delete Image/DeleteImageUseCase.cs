
using Forum.Domain.Repository;
using Forum.Domain.Repository.User;
using Forum.Domain.Services;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;

namespace Forum.Application.UseCases.User.Delete_Image
{
    public class DeleteImageUseCase(
        ILoggedUser loggedUser,
        IUserUpdateOnlyRepository userUpdateOnlyRepository,
        IPhotoService photoService,
        IUnitOfWork unitOfWork) : IDeleteImageUseCase
    {
        private readonly ILoggedUser _loggedUser = loggedUser;
        private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository = userUpdateOnlyRepository;
        private readonly IPhotoService _photoService = photoService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Execute()
        {
            var loggedUser = await _loggedUser.User();

            var user = await _userUpdateOnlyRepository.GetById(loggedUser.Id);

            if (string.IsNullOrWhiteSpace(user.ImageIdentifier) && string.IsNullOrWhiteSpace(user.ImageUrl))
            {
                throw new NotFoundException(ResourceMessagesException.USER_DOES_NOT_HAVE_PHOTO);
            }

            await _photoService.DeleteImage(user.ImageIdentifier!);

            user.ImageIdentifier = null;
            user.ImageUrl = null;

            _userUpdateOnlyRepository.Update(user);
            await _unitOfWork.Commit();
        }
    }
}
