using FluentAssertions;
using Forum.Communication.Request;
using Forum.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Token
{
    public class GetNewAccessTokenTest : ForumClassFixture
    {
        private readonly string _endpoint = "token/refresh-token";
        private readonly string _refreshToken;

        public GetNewAccessTokenTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _refreshToken = factory.GetRefreshToken();
        }

        [Fact]
        public async Task Success()
        {
            var request = new RequestNewTokenJson
            {
                RefreshToken = _refreshToken,
            };

            var response = await DoPost(_endpoint, request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("accessToken").GetString().Should().NotBeNullOrEmpty();
            responseData.RootElement.GetProperty("refreshToken").GetString().Should().NotBeNullOrEmpty();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Refresh_Token_Not_Found(string culture)
        {
            var request = new RequestNewTokenJson
            {
                RefreshToken = "invalidToken",
            };

            var response = await DoPost(_endpoint, request, culture: culture);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var error = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("TOKEN_NOT_FOUND", new CultureInfo(culture));

            error.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
