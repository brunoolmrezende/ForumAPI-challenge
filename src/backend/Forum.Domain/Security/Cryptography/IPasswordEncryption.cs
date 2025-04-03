namespace Forum.Domain.Security.Cryptography
{
    public interface IPasswordEncryption
    {
        public string Encrypt(string password);
        public bool Verify(string password, string hashedPassword);
    }
}
