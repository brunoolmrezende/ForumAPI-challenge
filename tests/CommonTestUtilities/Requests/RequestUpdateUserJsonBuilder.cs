using Bogus;
using Forum.Communication.Request;

namespace CommonTestUtilities.Requests
{
    public class RequestUpdateUserJsonBuilder
    {
        public static RequestUpdateUserJson Build()
        {
            return new Faker<RequestUpdateUserJson>()
                .RuleFor(user => user.Name, (f) => f.Person.FirstName)
                .RuleFor(user => user.Email, (f, user) => f.Internet.Email(user.Name));
        }
    }
}
