using Forum.Domain.Services;
using Moq;

namespace CommonTestUtilities.Services.Queue
{
    public class DeleteUserQueueBuilder
    {
        public static IDeleteUserQueue Build()
        {
            var mock = new Mock<IDeleteUserQueue>();

            return mock.Object;
        }
    }   
}
