using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using Forum.Communication.Request;
using Forum.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Change_Password
{
    public class ChangePasswordTest : ForumClassFixture
    {
        private readonly string _endpoint = "user/change-password";
        private readonly string _oldPassword;
        private readonly Guid _identifier;
        private readonly string _email;

        public ChangePasswordTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _oldPassword = factory.GetPassword();
            _identifier = factory.GetIdentifier();
            _email = factory.GetEmail();
        }

        [Fact]
        public async Task Success()
        {
            var token = AccessTokenGeneratorBuilder.Build().Generate(_identifier);

            var request = RequestChangePasswordJsonBuilder.Build();
            request.OldPassword = _oldPassword;

            var response = await DoPut(_endpoint, request, token);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var loginRequest = new RequestDoLoginJson
            {
                Email = _email,
                Password = _oldPassword
            };

            response = await DoPost("auth/login", request, token);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            loginRequest.Password = request.NewPassword;

            response = await DoPost("auth/login", loginRequest, token);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_New_Password_Empty(string culture)
        {
            var request = new RequestChangePasswordJson
            {
                NewPassword = string.Empty,
                OldPassword = _oldPassword,
            };

            var token = AccessTokenGeneratorBuilder.Build().Generate(_identifier);

            var response = await DoPut(_endpoint, request, token, culture);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMPTY_PASSWORD", new CultureInfo(culture));

            errors.Should().ContainSingle().Which.GetString().Should().Be(expectedMessage);
            errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
