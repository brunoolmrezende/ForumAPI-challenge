using Forum.Domain.Services;
using Moq;

namespace CommonTestUtilities.LoggedUser
{
    public class LoggedUserBuilder
    {
        public static ILoggedUser Build(Forum.Domain.Entities.User user)
        {
            var mock = new Mock<ILoggedUser>();

            mock.Setup(x => x.User()).ReturnsAsync(user);

            return mock.Object;
        }
    }
}
