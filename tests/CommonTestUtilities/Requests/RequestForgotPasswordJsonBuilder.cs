using Bogus;
using Forum.Communication.Request;

namespace CommonTestUtilities.Requests
{
    public static class RequestForgotPasswordJsonBuilder
    {
        public static RequestForgotPasswordJson Build(string email)
        {
            return new Faker<RequestForgotPasswordJson>()
                .RuleFor(x => x.Email, _ => email);
        }
    }
}
