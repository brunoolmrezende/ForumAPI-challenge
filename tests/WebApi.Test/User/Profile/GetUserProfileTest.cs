using CommonTestUtilities.Tokens;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.User.Profile
{
    public class GetUserProfileTest : ForumClassFixture
    {
        private readonly string _endpoint = "user";
        private readonly Guid _identifier;
        private readonly string _name;
        private readonly string _email;
        private readonly string _imageUrl;
        private readonly int _topicsCount;
        private readonly int _commentsCount;

        public GetUserProfileTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _identifier = factory.GetIdentifier();
            _name = factory.GetName();
            _email = factory.GetEmail();
            _imageUrl = factory.GetImageUrl();
            _topicsCount = factory.GetTopicsCount();
            _commentsCount = factory.GetCommentsCount();
        }

        [Fact]
        public async Task Success()
        {
            var token = AccessTokenGeneratorBuilder.Build().Generate(_identifier);

            var response = await DoGet(_endpoint, token);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("name").GetString().Should().Be(_name);
            responseData.RootElement.GetProperty("email").GetString().Should().Be(_email);
            responseData.RootElement.GetProperty("imageUrl").GetString().Should().Be(_imageUrl);
            responseData.RootElement.GetProperty("topicsCount").GetInt32().Should().Be(_topicsCount);
            responseData.RootElement.GetProperty("commentsCount").GetInt32().Should().Be(_commentsCount);
        }
    }
}
