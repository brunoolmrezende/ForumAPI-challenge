using Forum.Domain.Entities;
using Forum.Domain.Repository.Token;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class TokenRepositoryBuilder
    {
        private readonly Mock<ITokenRepository> _mock;

        public TokenRepositoryBuilder() => _mock = new Mock<ITokenRepository>();

        public TokenRepositoryBuilder GetToken(RefreshToken? refreshToken)
        {
            if (refreshToken is not null)
            {
                _mock.Setup(repository => repository.GetToken(refreshToken.Value)).ReturnsAsync(refreshToken);
            }

            return this;
        }

        public ITokenRepository Build() => _mock.Object;
    }
}
