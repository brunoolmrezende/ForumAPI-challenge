using CommonTestUtilities.Tokens;
using FluentAssertions;
using Forum.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Topic.GetById
{
    public class GetTopicByIdTest : ForumClassFixture
    {
        private const string _endpoint = "topic";
        private readonly Guid _identifier;
        private readonly Forum.Domain.Entities.Topic _topic;

        public GetTopicByIdTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _topic = factory.GetTopic();
            _identifier = factory.GetIdentifier();
        }

        [Fact]
        public async Task Success_Logged_In()
        {
            var token = AccessTokenGeneratorBuilder.Build().Generate(_identifier);

            var response = await DoGet($"{_endpoint}/{_topic.Id}", token);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("id").GetInt64().Should().Be(_topic.Id);
            responseData.RootElement.GetProperty("title").GetString().Should().Be(_topic.Title);
            responseData.RootElement.GetProperty("content").GetString().Should().Be(_topic.Content);
            responseData.RootElement.GetProperty("createdOn").GetDateTime().Should().Be(_topic.CreatedOn);

            responseData.RootElement.GetProperty("user").GetProperty("id").GetInt64().Should().Be(_topic.User.Id);
            responseData.RootElement.GetProperty("user").GetProperty("name").GetString().Should().Be(_topic.User.Name);

            responseData.RootElement.GetProperty("totalLikes").GetInt32().Should().Be(_topic.Likes.Count);
            responseData.RootElement.GetProperty("likedByCurrentUser").GetBoolean().Should().BeTrue();

            var comment = responseData.RootElement.GetProperty("comments").EnumerateArray().First();

            comment.GetProperty("id").GetInt64().Should().Be(_topic.Comments.First().Id);
            comment.GetProperty("author").GetString().Should().Be(_topic.Comments.First().User.Name);
            comment.GetProperty("content").GetString().Should().Be(_topic.Comments.First().Content);
            comment.GetProperty("createdOn").GetDateTime().Should().Be(_topic.Comments.First().CreatedOn);
        }

        [Fact]
        public async Task Success__Not_Logged_In()
        {
            var response = await DoGet($"{_endpoint}/{_topic.Id}", token: null!);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("id").GetInt64().Should().Be(_topic.Id);
            responseData.RootElement.GetProperty("title").GetString().Should().Be(_topic.Title);
            responseData.RootElement.GetProperty("content").GetString().Should().Be(_topic.Content);
            responseData.RootElement.GetProperty("createdOn").GetDateTime().Should().Be(_topic.CreatedOn);

            responseData.RootElement.GetProperty("user").GetProperty("id").GetInt64().Should().Be(_topic.User.Id);
            responseData.RootElement.GetProperty("user").GetProperty("name").GetString().Should().Be(_topic.User.Name);

            responseData.RootElement.GetProperty("totalLikes").GetInt32().Should().Be(_topic.Likes.Count);
            responseData.RootElement.GetProperty("likedByCurrentUser").GetBoolean().Should().BeFalse();

            var comment = responseData.RootElement.GetProperty("comments").EnumerateArray().First();

            comment.GetProperty("id").GetInt64().Should().Be(_topic.Comments.First().Id);
            comment.GetProperty("author").GetString().Should().Be(_topic.Comments.First().User.Name);
            comment.GetProperty("content").GetString().Should().Be(_topic.Comments.First().Content);
            comment.GetProperty("createdOn").GetDateTime().Should().Be(_topic.Comments.First().CreatedOn);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Topic_Not_Found(string culture)
        {
            var topicId = 1000;

            var response = await DoGet($"{_endpoint}/{topicId}", token: null!, culture);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("TOPIC_NOT_FOUND", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
