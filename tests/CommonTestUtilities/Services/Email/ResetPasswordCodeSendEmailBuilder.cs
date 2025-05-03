using Forum.Domain.Services;
using Moq;

namespace CommonTestUtilities.Services.Email
{
    public class ResetPasswordCodeSendEmailBuilder
    {
        public static IResetPasswordCodeSendEmail Build()
        {
            var mock = new Mock<IResetPasswordCodeSendEmail>();

            return mock.Object;
        }
    }
}
