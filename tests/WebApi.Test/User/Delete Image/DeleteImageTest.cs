using CommonTestUtilities.Tokens;
using FluentAssertions;
using Forum.Exceptions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Delete_Image
{
    public class DeleteImageTest : ForumClassFixture
    {
        private readonly string _endpoint = "user/delete-photo";
        private readonly Guid _identifier;
        private readonly Guid _identifierFromUserWithoutPhoto;

        public DeleteImageTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _identifier = factory.GetIdentifier();
            _identifierFromUserWithoutPhoto = factory.GetIdentifierFromUserWithoutPhoto();
        }

        [Fact]
        public async Task Success()
        {
            var token = AccessTokenGeneratorBuilder.Build().Generate(_identifier);

            var response = await DoDelete(_endpoint, token);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_User_Does_Not_Have_Photo(string culture)
        {
            var token = AccessTokenGeneratorBuilder.Build().Generate(_identifierFromUserWithoutPhoto);

            var response = await DoDelete(_endpoint, token, culture);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("USER_DOES_NOT_HAVE_PHOTO", new CultureInfo(culture));

            errors.Should().ContainSingle().And.Contain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
