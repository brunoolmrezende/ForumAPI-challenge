using Bogus;
using Forum.Communication.Request;
using Microsoft.AspNetCore.Http;

namespace CommonTestUtilities.Requests
{
    public class RequestRegisterUserFormDataBuilder
    {
        public static RequestRegisterUserFormData Build(IFormFile? file = null, int passwordLength = 10)
        {
            return new Faker<RequestRegisterUserFormData>()
                .RuleFor(user => user.Image, _ => file)
                .RuleFor(user => user.Name, (f) => f.Person.FirstName)
                .RuleFor(user => user.Email, (f, user) => f.Internet.Email(user.Name))
                .RuleFor(user => user.Password, (f) => f.Internet.Password(passwordLength));
        }
    }
}
