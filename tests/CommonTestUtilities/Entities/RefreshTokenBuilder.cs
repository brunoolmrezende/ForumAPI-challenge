using Bogus;
using Forum.Domain.Entities;

namespace CommonTestUtilities.Entities
{
    public class RefreshTokenBuilder
    {
        public static RefreshToken Build(User user)
        {
            return new Faker<RefreshToken>()
                .RuleFor(r => r.Id, _ => 1)
                .RuleFor(r => r.Value, (f) => f.Lorem.Word())
                .RuleFor(r => r.CreatedOn, _ => DateTime.UtcNow)
                .RuleFor(r => r.UserId, _ => user.Id)
                .RuleFor(r => r.User, _ => user);
        }
    }
}
