namespace Forum.Domain.Services
{
    public interface IResetPasswordCodeSendEmail
    {
        Task SendEmail(string to, string subject, string body);
    }
}
