using CommonTestUtilities.Tokens;
using FluentAssertions;
using Forum.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Like.Comment
{
    public class ToggleCommentLikeTest : ForumClassFixture
    {
        private readonly string _endpoint = "like/comment";
        private readonly long _commentId;
        private readonly Guid _identifier;
        public ToggleCommentLikeTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _commentId = factory.GetCommentId();
            _identifier = factory.GetIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            var token = AccessTokenGeneratorBuilder.Build().Generate(_identifier);

            var response = await DoPost(endpoint: $"{_endpoint}/{_commentId}", request: null, token: token);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Comment_Not_Found(string culture)
        {
            var token = AccessTokenGeneratorBuilder.Build().Generate(_identifier);

            var commentId = 1000;

            var response = await DoPost(endpoint: $"{_endpoint}/{commentId}", request: null, token: token, culture);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("COMMENT_NOT_FOUND", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
