using Bogus;
using Forum.Communication.Request;

namespace CommonTestUtilities.Requests
{
    public class RequestResetPasswordJsonBuilder
    {
        public static RequestResetPasswordJson Build(string email, string codeValue)
        {
            return new Faker<RequestResetPasswordJson>()
                .RuleFor(x => x.Code, _ => codeValue)
                .RuleFor(x => x.Email, _ => email)
                .RuleFor(x => x.NewPassword, f => f.Internet.Password());
        }
    }
}
