
using Forum.Domain.Repository;
using Forum.Domain.Repository.User;
using Forum.Domain.Services;

namespace Forum.Application.UseCases.User.Delete.Delete_User_Account
{
    public class DeleteUserAccountUseCase(
        IUserDeleteOnlyRepository deleteOnlyRepository,
        IUserReadOnlyRepository readOnlyRepository,
        IPhotoService photoService,
        IUnitOfWork unitOfWork) : IDeleteUserAccountUseCase
    {
        private readonly IUserDeleteOnlyRepository _deleteOnlyRepository = deleteOnlyRepository;
        private readonly IUserReadOnlyRepository _readOnlyRepository = readOnlyRepository;
        private readonly IPhotoService _photoService = photoService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Execute(Guid userIdentifier)
        {
            var user = await _readOnlyRepository.GetByIdentifier(userIdentifier);

            if (!string.IsNullOrWhiteSpace(user!.ImageIdentifier))
            {
                await _photoService.DeleteImage(user.ImageIdentifier);
            }

            await _deleteOnlyRepository.DeleteAccount(userIdentifier);
            await _unitOfWork.Commit();
        }
    }
}
