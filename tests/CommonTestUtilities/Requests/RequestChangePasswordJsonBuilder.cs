using Bogus;
using Forum.Communication.Request;

namespace CommonTestUtilities.Requests
{
    public class RequestChangePasswordJsonBuilder
    {
        public static RequestChangePasswordJson Build(int passwordLength = 8) 
        {
            return new Faker<RequestChangePasswordJson>()
                .RuleFor(RequestChangePasswordJson => RequestChangePasswordJson.OldPassword, (f) => f.Internet.Password())
                .RuleFor(request => request.NewPassword, (f) => f.Internet.Password(passwordLength));
        }
    }
}
