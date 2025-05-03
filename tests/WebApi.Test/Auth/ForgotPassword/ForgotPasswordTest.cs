using FluentAssertions;
using Forum.Communication.Request;
using Forum.Domain.ValueObjects;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Auth.ForgotPassword
{
    public class ForgotPasswordTest : ForumClassFixture
    {
        private readonly string _endpoint = "auth/forgot-password";
        private readonly string _userEmail;

        public ForgotPasswordTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userEmail = factory.GetEmail();
        }

        [Fact]
        public async Task Success()
        {
            var request = new RequestForgotPasswordJson
            {
                Email = _userEmail,
            };

            var response = await DoPost(_endpoint, request);

            response.StatusCode.Should().Be(HttpStatusCode.Accepted);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("message")
                .GetString()
                .Should()
                .Be(ResourceMessage.GENERIC_MESSAGE);
        }
    }
}
