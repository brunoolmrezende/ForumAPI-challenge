using Forum.Domain.Security.ResetPasswordCode;
using System.Security.Cryptography;

namespace Forum.Infrastructure.Security.ResetPasswordCode
{
    public class ResetPasswordCodeGenerator : IResetPasswordCodeGenerator
    {
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private const int _length = 6;

        public string Generate()
        {
            var code = new char[_length];
            using var rng = RandomNumberGenerator.Create();
            byte[] randomBytes = new byte[_length];

            rng.GetBytes(randomBytes);

            for (int i = 0; i < _length; i++)
            {
                int index = randomBytes[i] % _chars.Length;
                code[i] = _chars[index];
            }

            return new string(code);
        }
    }
}
