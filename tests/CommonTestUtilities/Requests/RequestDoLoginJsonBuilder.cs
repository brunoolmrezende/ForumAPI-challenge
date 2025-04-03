using Bogus;
using Forum.Communication.Request;

namespace CommonTestUtilities.Requests
{
    public class RequestDoLoginJsonBuilder
    {
        public static RequestDoLoginJson Build(int passwordLength = 8)
        {
            return new Faker<RequestDoLoginJson>()
                .RuleFor(user => user.Email, (f) => f.Internet.Email())
                .RuleFor(user => user.Password, (f) => f.Internet.Password(passwordLength));
        }
    }
}
