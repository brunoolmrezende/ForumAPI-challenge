using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using Forum.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Comment.Register
{
    public class RegisterCommentTest : ForumClassFixture
    {
        private const string _endpoint = "comment";
        private readonly Guid _identifier;
        private readonly long _topicId;

        public RegisterCommentTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _identifier = factory.GetIdentifier();
            _topicId = factory.GetTopicId();
        }

        [Fact]
        public async Task Success()
        {
            var token = AccessTokenGeneratorBuilder.Build().Generate(_identifier);

            var request = RequestCommentJsonBuilder.Build();

            var response = await DoPost(endpoint: $"{_endpoint}/{_topicId}", request, token);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("id").GetInt64().Should().NotBe(0);
            responseData.RootElement.GetProperty("content").GetString().Should().Be(request.Content);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Topic_Not_Found(string culture)
        {
            var token = AccessTokenGeneratorBuilder.Build().Generate(_identifier);

            var request = RequestCommentJsonBuilder.Build();

            var topicId = 1000;

            var response = await DoPost(endpoint: $"{_endpoint}/{topicId}", request, token, culture);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("TOPIC_NOT_FOUND", new CultureInfo(culture));

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
