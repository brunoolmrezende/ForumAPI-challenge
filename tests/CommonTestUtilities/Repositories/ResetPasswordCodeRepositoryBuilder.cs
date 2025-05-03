using Forum.Domain.Repository.ResetPasswordCode;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class ResetPasswordCodeRepositoryBuilder
    {
        private readonly Mock<IResetPasswordCodeRepository> _mock;

        public ResetPasswordCodeRepositoryBuilder()
        {
            _mock = new Mock<IResetPasswordCodeRepository>();
        }

        public IResetPasswordCodeRepository Build() => _mock.Object;        
    }
}
