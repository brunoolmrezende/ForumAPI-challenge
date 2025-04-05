using Bogus;

namespace CommonTestUtilities.Entities
{
    public class TopicBuilder
    {
        public static Forum.Domain.Entities.Topic Build(Forum.Domain.Entities.User user)
        {
            return new Faker<Forum.Domain.Entities.Topic>()
                .RuleFor(topic => topic.Id, _ => 1)
                .RuleFor(topic => topic.Title, f => f.Lorem.Sentence(5))
                .RuleFor(topic => topic.Content, f => f.Lorem.Paragraph(3))
                .RuleFor(topic => topic.UserId, _ => user.Id);
        }
    }
}
