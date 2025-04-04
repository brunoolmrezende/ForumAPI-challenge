namespace Forum.Domain.Security.AccessToken
{
    public interface IAccessTokenGenerator
    {
        public string Generate(Guid userIdentifier);
    }
}
