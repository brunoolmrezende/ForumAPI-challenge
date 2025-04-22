namespace Forum.Application.UseCases.User.Delete.Delete_User_Account
{
    public interface IDeleteUserAccountUseCase
    {
        Task Execute(Guid userIdentifier);
    }
}
