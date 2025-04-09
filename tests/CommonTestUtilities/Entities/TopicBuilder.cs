using Bogus;
using Forum.Domain.Entities;

namespace CommonTestUtilities.Entities
{
    public class TopicBuilder
    {
        public static List<Topic> Collection(User user, uint count = 2)
        {
            var list = new List<Topic>();

            if (count == 0)
                count = 1;

            var topicId = 1;

            for (int i = 0; i < count; i++)
            {
                var fakeTopic = Build(user);
                fakeTopic.Id = topicId++;

                list.Add(fakeTopic);
            }

            return list;
        }

        public static Topic Build(User user)
        {
            return new Faker<Topic>()
                .RuleFor(topic => topic.Id, _ => 1)
                .RuleFor(topic => topic.Title, f => f.Lorem.Sentence(5))
                .RuleFor(topic => topic.Content, f => f.Lorem.Paragraph(3))
                .RuleFor(topic => topic.UserId, _ => user.Id);
        }
    }
}
