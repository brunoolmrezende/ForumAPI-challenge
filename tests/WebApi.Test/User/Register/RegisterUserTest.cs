﻿using CommonTestUtilities.Requests;
using FluentAssertions;
using Forum.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register
{
    public class RegisterUserTest : ForumClassFixture
    {
        private readonly string _endpoint = "user";

        public RegisterUserTest(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var response = await DoPost(_endpoint, request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
            responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().Should().NotBeNullOrEmpty();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empty_Name(string culture)
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            var response = await DoPost(endpoint: _endpoint, request: request, culture: culture);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
