using Forum.Domain.Security.Cryptography;

namespace Forum.Infrastructure.Cryptography
{
    public class BCryptNet : IPasswordEncryption
    {
        public string Encrypt(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
