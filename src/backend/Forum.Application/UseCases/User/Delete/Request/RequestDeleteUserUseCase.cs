using Forum.Domain.Repository;
using Forum.Domain.Repository.User;
using Forum.Domain.Services;

namespace Forum.Application.UseCases.User.Delete.Request
{
    public class RequestDeleteUserUseCase(
        ILoggedUser loggedUser,
        IUserUpdateOnlyRepository repository,
        IUnitOfWork unitOfWork,
        IDeleteUserQueue deleteUserQueue) : IRequestDeleteUserUseCase
    {
        private readonly ILoggedUser _loggedUser = loggedUser;
        private readonly IUserUpdateOnlyRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IDeleteUserQueue _deleteUserQueue = deleteUserQueue;

        public async Task Execute()
        {
            var loggedUser = await _loggedUser.User();

            var user = await _repository.GetById(loggedUser.Id);
            user.Active = false;

            _repository.Update(user);
            await _unitOfWork.Commit();

            await _deleteUserQueue.SendMessage(user);
        }
    }
}
