using Forum.Domain.Security.Cryptography;
using BC = BCrypt.Net;

namespace Forum.Infrastructure.Security.Cryptography
{
    public class BCryptNet : IPasswordEncryption
    {
        public string Encrypt(string password)
        {
            return BC.BCrypt.HashPassword(password);
        }

        public bool Verify(string password, string hashedPassword)
        {
            return BC.BCrypt.Verify(password, hashedPassword);
        }
    }
}
