using Bogus;
using Forum.Domain.Entities;

namespace CommonTestUtilities.Entities
{
    public class ResetPasswordCodeBuilder
    {
        public static ResetPasswordCode Build(string userEmail)
        {
            return new Faker<ResetPasswordCode>()
                .RuleFor(x => x.Id, _ => 1)
                .RuleFor(x => x.Value, f => f.Random.AlphaNumeric(6))
                .RuleFor(x => x.UserEmail, _ => userEmail)
                .RuleFor(x => x.Active, _ => true)
                .RuleFor(x => x.CreatedOn, _ => DateTime.UtcNow);
        }
    }
}
