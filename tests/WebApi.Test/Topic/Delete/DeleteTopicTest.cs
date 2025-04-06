using CommonTestUtilities.Tokens;
using FluentAssertions;
using Forum.Exceptions;
using System.Globalization;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Topic.Delete
{
    public class DeleteTopicTest : ForumClassFixture
    {
        private const string _endpoint = "topic";
        private readonly Guid _identifier;
        private readonly long _topicId;

        public DeleteTopicTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _topicId = factory.GetTopicId();
            _identifier = factory.GetIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            var token = AccessTokenGeneratorBuilder.Build().Generate(_identifier);

            var response = await DoDelete(endpoint: $"{_endpoint}/{_topicId}", token);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Topic_Not_Found(string culture)
        {
            var token = AccessTokenGeneratorBuilder.Build().Generate(_identifier);

            var topicId = 1000;

            var response = await DoDelete(endpoint: $"{_endpoint}/{topicId}", token, culture);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("TOPIC_NOT_FOUND", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
