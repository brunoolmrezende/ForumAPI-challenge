using CommonTestUtilities.Requests;
using FluentAssertions;
using Forum.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register
{
    public class RegisterUserTest(CustomWebApplicationFactory factory) : ForumClassFixture(factory)
    {
        private readonly string _endpoint = "user";

        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserFormDataBuilder.Build();

            var response = await DoPostFormData(endpoint: _endpoint, request: request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
            responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().Should().NotBeNullOrEmpty();
            responseData.RootElement.GetProperty("tokens").GetProperty("refreshToken").GetString().Should().NotBeNullOrEmpty();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empty_Name(string culture)
        {
            var request = RequestRegisterUserFormDataBuilder.Build();
            request.Name = string.Empty;

            var response = await DoPostFormData(endpoint: _endpoint, request: request, culture: culture);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
