using Forum.Domain.Entities;
using Forum.Domain.Repository.User;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class UserReadOnlyRepositoryBuilder
    {
        private readonly Mock<IUserReadOnlyRepository> _mock;
        public UserReadOnlyRepositoryBuilder() => _mock = new Mock<IUserReadOnlyRepository>();

        public UserReadOnlyRepositoryBuilder ExistActiveUserWithEmail(string? email = null)
        {
            if (email is not null)
            {
                _mock.Setup(repository => repository.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
            }

            return this;
        }

        public UserReadOnlyRepositoryBuilder GetByEmail(User user)
        {
            _mock.Setup(repository => repository.GetByEmail(user.Email)).ReturnsAsync(user);

            return this;
        }

        public IUserReadOnlyRepository Build() => _mock.Object;
    }
}
