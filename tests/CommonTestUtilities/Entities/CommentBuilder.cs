using Bogus;
using Forum.Domain.Entities;

namespace CommonTestUtilities.Entities
{
    public class CommentBuilder
    {
        public static Comment Build(User user, long topicId)
        {
            return new Faker<Comment>()
                .RuleFor(comment => comment.Id, _ => 1)
                .RuleFor(comment => comment.Content, f => f.Lorem.Paragraph(1))
                .RuleFor(comment => comment.UserId, _ => user.Id)
                .RuleFor(comment => comment.TopicId, _ => topicId);
        }
    }
}
