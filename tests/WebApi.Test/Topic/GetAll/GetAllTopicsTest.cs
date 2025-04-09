using CommonTestUtilities.Tokens;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Topic.GetAll
{
    public class GetAllTopicsTest : ForumClassFixture
    {
        private const string _endpoint = "forum";
        private readonly Guid _identifier;

        public GetAllTopicsTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _identifier = factory.GetIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            var token = AccessTokenGeneratorBuilder.Build().Generate(_identifier);

            var response = await DoGet(_endpoint, token);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("topics").EnumerateArray().Should().NotBeNullOrEmpty();  
        }
    }
}
