namespace Forum.Domain.Security.RefreshToken
{
    public interface IRefreshTokenGenerator
    {
        public string Generate();
    }
}
