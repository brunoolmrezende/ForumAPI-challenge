using Forum.Domain.Security.ResetPasswordCode;
using Forum.Infrastructure.Security.ResetPasswordCode;

namespace CommonTestUtilities.Tokens
{
    public class ResetPasswordCodeGeneratorBuilder
    {
        public static IResetPasswordCodeGenerator Build() => new ResetPasswordCodeGenerator();
    }
}
