using Bogus;
using Forum.Communication.Request;

namespace CommonTestUtilities.Requests
{
    public class RequestCommentJsonBuilder
    {
        public static RequestCommentJson Build()
        {
            return new Faker<RequestCommentJson>()
                .RuleFor(comment => comment.Content, (f) => f.Lorem.Paragraphs(1));
        }
    }
}
