using Forum.Domain.Services;
using System.Net.Mail;
using System.Net;

namespace Forum.Infrastructure.Services.Email
{
    public class ResetPasswordCodeSendEmail(
        string credentialUser,
        string credentialPassword,
        string address,
        string displayName,
        string host,
        int port) : IResetPasswordCodeSendEmail
    {
        private readonly string _credentialUser = credentialUser;
        private readonly string _credentialPassword = credentialPassword;
        private readonly string _address = address;
        private readonly string _displayName = displayName;
        private readonly string _host = host;
        private readonly int _port = port;

        public async Task SendEmail(string to, string subject, string body)
        {
            using var client = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_credentialUser, _credentialPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(_address, _displayName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(to);

            await client.SendMailAsync(mailMessage);
        }
    }
}
