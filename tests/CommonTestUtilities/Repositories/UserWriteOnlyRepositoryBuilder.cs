using Forum.Domain.Repository.User;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class UserWriteOnlyRepositoryBuilder
    {
        private readonly Mock<IUserWriteOnlyRepository> _mock;

        public UserWriteOnlyRepositoryBuilder() => _mock = new Mock<IUserWriteOnlyRepository>();

        public IUserWriteOnlyRepository Build() => _mock.Object;
    }
}
