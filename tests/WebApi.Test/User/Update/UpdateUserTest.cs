using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using Forum.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Update
{
    public class UpdateUserTest : ForumClassFixture
    {
        private readonly string _endpoint = "user";
        private readonly Guid _identifier;
        public UpdateUserTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _identifier = factory.GetIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            var token = AccessTokenGeneratorBuilder.Build().Generate(_identifier);

            var request = RequestUpdateUserJsonBuilder.Build();

            var response = await DoPut(_endpoint, request, token);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empty_Name(string culture)
        {
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = string.Empty;

            var token = AccessTokenGeneratorBuilder.Build().Generate(_identifier);

            var response = await DoPut(_endpoint, request, token, culture);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expecteMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

            errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expecteMessage));
        }
    }
}
