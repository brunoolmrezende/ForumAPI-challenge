using Forum.Domain.Repository.User;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class UserUpdateOnlyRepositoryBuilder
    {
        private readonly Mock<IUserUpdateOnlyRepository> _mock;
        public UserUpdateOnlyRepositoryBuilder()
        {
            _mock = new Mock<IUserUpdateOnlyRepository>();
        }
        public UserUpdateOnlyRepositoryBuilder GetById(Forum.Domain.Entities.User user)
        {
            _mock.Setup(repository => repository.GetById(user.Id)).ReturnsAsync(user);

            return this;
        }

        public UserUpdateOnlyRepositoryBuilder GetByEmail(Forum.Domain.Entities.User? user = null)
        {
            if (user is not null)
            {
                _mock.Setup(repository => repository.GetByEmail(user.Email)).ReturnsAsync(user);
            }

            return this;
        }

        public IUserUpdateOnlyRepository Build() => _mock.Object;
    }
}
