using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using Forum.Communication.Request;
using Forum.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Topic.Filter
{
    public class FilterTopicUseCase : ForumClassFixture
    {
        private const string _endpoint = "topic/filter";
        private readonly Guid _identifier;
        private readonly string _topicTitle;
        private readonly string _topicContent;

        public FilterTopicUseCase(CustomWebApplicationFactory factory) : base(factory)
        {
            _identifier = factory.GetIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            var token = AccessTokenGeneratorBuilder.Build().Generate(_identifier);

            var request = new RequestFilterTopicJson
            {
                Title = _topicTitle,
                Content = _topicContent,
            };

            var response = await DoPost(_endpoint, request, token);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("topics").EnumerateArray().Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Success_No_Content()
        {
            var token = AccessTokenGeneratorBuilder.Build().Generate(_identifier);

            var request = RequestFilterTopicJsonBuilder.Build();
            request.Title = "TopicDoesntExist";

            var response = await DoPost(_endpoint, request, token);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Topic_Title_Max_Length(string culture)
        {
            var token = AccessTokenGeneratorBuilder.Build().Generate(_identifier);

            var request = RequestFilterTopicJsonBuilder.Build();
            request.Title = new string('a', 256);

            var response = await DoPost(_endpoint, request, token, culture);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("TITLE_MAX_LENGTH", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
