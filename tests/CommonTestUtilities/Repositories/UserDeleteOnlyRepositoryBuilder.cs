using Forum.Domain.Repository.User;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class UserDeleteOnlyRepositoryBuilder
    {
        private readonly Mock<IUserDeleteOnlyRepository> _mock;

        public UserDeleteOnlyRepositoryBuilder() => _mock = new Mock<IUserDeleteOnlyRepository>();
        

        public IUserDeleteOnlyRepository Build() => _mock.Object;
    }
}
