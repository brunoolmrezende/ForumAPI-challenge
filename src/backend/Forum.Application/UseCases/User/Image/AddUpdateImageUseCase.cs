using Forum.Application.Extensions;
using Forum.Domain.Repository;
using Forum.Domain.Repository.User;
using Forum.Domain.Services;
using Forum.Exceptions;
using Forum.Exceptions.ExceptionBase;
using Microsoft.AspNetCore.Http;

namespace Forum.Application.UseCases.User.Image
{
    public class AddUpdateImageUseCase(
        ILoggedUser loggedUser,
        IPhotoService photoService,
        IUnitOfWork unitOfWork,
        IUserUpdateOnlyRepository userUpdateOnlyRepository) : IAddUpdateImageUseCase
    {
        private readonly ILoggedUser _loggedUser = loggedUser;
        private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository = userUpdateOnlyRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPhotoService _photoService = photoService;

        public async Task Execute(IFormFile file)
        {
            var loggedUser = await _loggedUser.User();

            var user = await _userUpdateOnlyRepository.GetById(loggedUser.Id);

            var fileStream = file.OpenReadStream();

            var isValidImage = fileStream.ValidateImageExtension();

            if (isValidImage is false)
            {
                throw new ErrorOnValidationException([ResourceMessagesException.ONLY_IMAGES_ACCEPTED]);
            }

            if (!string.IsNullOrEmpty(user.ImageIdentifier))
            {
                await _photoService.DeleteImage(user.ImageIdentifier);
            }

            var uploadResult = await _photoService.UploadImage(file, user, filename: Guid.NewGuid().ToString());

            user.ImageUrl = uploadResult.Url;
            user.ImageIdentifier = uploadResult.PublicId;

            _userUpdateOnlyRepository.Update(user);
            await _unitOfWork.Commit();
        }
    }
}
