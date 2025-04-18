namespace Forum.Domain.Repository.Token
{
    public interface ITokenRepository
    {
        Task<Entities.RefreshToken?> GetToken(string refreshToken);
        Task SaveNewRefreshToken(Entities.RefreshToken refreshToken);
    }
}
