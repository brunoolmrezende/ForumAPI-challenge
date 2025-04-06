using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using Forum.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Topic.Delete
{
    public class DeleteTopicInvalidTokenTest : ForumClassFixture
    {
        private const string _endpoint = "topic";
        private readonly long _topicId;

        public DeleteTopicInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _topicId = factory.GetTopicId();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Token_Empty(string culture)
        {
            var request = RequestTopicJsonBuilder.Build();

            var response = await DoPut(endpoint: $"{_endpoint}/{_topicId}", request: request, token: string.Empty, culture: culture);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NO_TOKEN", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Token_Invalid(string culture)
        {
            var request = RequestTopicJsonBuilder.Build();

            var response = await DoPut(endpoint: $"{_endpoint}/{_topicId}", request: request, token: "invalidToken", culture: culture);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("USER_WITHOUT_PERMISSION_ACCESS_RESOURCE", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Token_With_User_Not_Found(string culture)
        {
            var request = RequestTopicJsonBuilder.Build();

            var token = AccessTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var response = await DoPut(endpoint: $"{_endpoint}/{_topicId}", request: request, token: token, culture: culture);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("USER_WITHOUT_PERMISSION_ACCESS_RESOURCE", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
