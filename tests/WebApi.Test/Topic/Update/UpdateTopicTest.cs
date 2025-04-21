using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using Forum.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Topic.Update
{
    public class UpdateTopicTest : ForumClassFixture
    {
        private const string _endpoint = "topic";
        private readonly Guid _identifier;
        private readonly long _topicId;

        public UpdateTopicTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _identifier = factory.GetIdentifier();
            _topicId = factory.GetTopicId();
        }

        [Fact]
        public async Task Success()
        {
            var token = AccessTokenGeneratorBuilder.Build().Generate(_identifier);

            var request = RequestTopicJsonBuilder.Build();

            var response = await DoPut(endpoint: $"{_endpoint}/{_topicId}", request: request, token: token);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Topic_Not_Found(string culture)
        {
            var token = AccessTokenGeneratorBuilder.Build().Generate(_identifier);

            var request = RequestTopicJsonBuilder.Build();

            var id = 1000;

            var response = await DoPut(endpoint: $"{_endpoint}/{id}", request: request, token: token, culture: culture);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("TOPIC_NOT_FOUND", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Title_Empty(string culture)
        {
            var token = AccessTokenGeneratorBuilder.Build().Generate(_identifier);

            var request = RequestTopicJsonBuilder.Build();
            request.Title = string.Empty;

            var response = await DoPut(endpoint: $"{_endpoint}/{_topicId}", request: request, token: token, culture: culture);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("TITLE_EMPTY", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Content_Empty(string culture)
        {
            var token = AccessTokenGeneratorBuilder.Build().Generate(_identifier);

            var request = RequestTopicJsonBuilder.Build();
            request.Content = string.Empty;

            var response = await DoPut(endpoint: $"{_endpoint}/{_topicId}", request: request, token: token, culture: culture);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("CONTENT_EMPTY", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
