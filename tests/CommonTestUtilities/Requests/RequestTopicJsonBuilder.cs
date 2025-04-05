using Bogus;
using Forum.Communication.Request;

namespace CommonTestUtilities.Requests
{
    public class RequestTopicJsonBuilder
    {
        public static RequestTopicJson Build()
        {
            return new Faker<RequestTopicJson>()
                .RuleFor(topic => topic.Title, f => f.Lorem.Sentence(5))
                .RuleFor(topic => topic.Content, f => f.Lorem.Paragraph(1));
        }
    }
}
