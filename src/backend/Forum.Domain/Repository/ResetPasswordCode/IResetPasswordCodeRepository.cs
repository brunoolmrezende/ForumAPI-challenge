namespace Forum.Domain.Repository.ResetPasswordCode
{
    public interface IResetPasswordCodeRepository
    {
        Task<Entities.ResetPasswordCode?> GetCode(string code, string email);
        Task SaveNewCode(Entities.ResetPasswordCode resetPasswordCode);
    }
}
