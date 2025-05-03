using CommonTestUtilities.Requests;
using FluentAssertions;
using Forum.Domain.ValueObjects;
using Forum.Exceptions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Auth.ResetPassword
{
    public class ResetPasswordTest : ForumClassFixture
    {
        private readonly string _endpoint = "auth/reset-password";
        private readonly string _userEmail;
        private readonly string _codeValue;
        private readonly string _expiredCodeValue;

        public ResetPasswordTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userEmail = factory.GetEmail();
            _codeValue = factory.GetResetPasswordCode();
            _expiredCodeValue = factory.GetExpiredResetPasswordCode();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestResetPasswordJsonBuilder.Build(_userEmail, _codeValue);

            var response = await DoPost(_endpoint, request);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("message").GetString().Should().Be(ResourceMessage.PASSWORD_CHANGED);
        }

        [Fact]
        public async Task Error_Code_Not_Found()
        {
            var request = RequestResetPasswordJsonBuilder.Build(_userEmail, "invalid-code");

            var response = await DoPost(_endpoint, request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(ResourceMessagesException.CODE_NOT_FOUND));
        }

        [Fact]
        public async Task Error_Code_Expired()
        {
            var request = RequestResetPasswordJsonBuilder.Build(_userEmail, _expiredCodeValue);

            var response = await DoPost(_endpoint, request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(ResourceMessagesException.EXPIRED_CODE));
        }
    }
}
