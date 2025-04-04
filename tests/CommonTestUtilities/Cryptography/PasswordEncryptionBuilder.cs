using Forum.Domain.Security.Cryptography;
using Forum.Infrastructure.Security.Cryptography;

namespace CommonTestUtilities.Cryptography
{
    public class PasswordEncryptionBuilder
    {
        public static IPasswordEncryption Build() => new BCryptNet();
    }
}
