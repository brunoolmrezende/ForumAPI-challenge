using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using Forum.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Delete_Image
{
    public class DeleteImageInvalidTokenTest(CustomWebApplicationFactory factory) : ForumClassFixture(factory)
    {
        private readonly string _endpoint = "user/delete-photo";

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Token_Empty(string culture)
        {
            var response = await DoDelete(_endpoint, token: string.Empty, culture);

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
            var response = await DoDelete(_endpoint, token: "invalidToken", culture);

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
            var token = AccessTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var response = await DoDelete(_endpoint, token, culture);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("USER_WITHOUT_PERMISSION_ACCESS_RESOURCE", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
