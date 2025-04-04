namespace Forum.Domain.Security.AccessToken
{
    public interface IAccessTokenValidator
    {
        public Guid ValidateAndGetUserIdentifier(string token);
    }
}
