using Forum.Communication.Request;
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

        public ResetPasswordCodeRepositoryBuilder GetCode(
            RequestResetPasswordJson request,
            Forum.Domain.Entities.ResetPasswordCode? resetPasswordCode = null)
        {
            if (resetPasswordCode is not null)
            {
                _mock.Setup(x => x.GetCode(request.Code, request.Email)).ReturnsAsync(resetPasswordCode);
            }

            return this;
        }
        public IResetPasswordCodeRepository Build() => _mock.Object;        
    }
}
