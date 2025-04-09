using Bogus;
using Forum.Communication.Request;

namespace CommonTestUtilities.Requests
{
    public class RequestFilterTopicJsonBuilder
    {
        public static RequestFilterTopicJson Build()
        {
            return new Faker<RequestFilterTopicJson>()
                .RuleFor(x => x.Title, f => f.Lorem.Sentence())
                .RuleFor(x => x.Content, f => f.Lorem.Paragraph())
                .RuleFor(x => x.OrderBy, f => f.PickRandom("title", "content"));
        }
    }
}
