using Bogus;
using Forum.Domain.Dtos;
using Forum.Domain.Entities;
using Forum.Domain.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace CommonTestUtilities.Services.Photo
{
    public class PhotoServiceBuilder
    {
        private readonly Mock<IPhotoService> _mock;

        public PhotoServiceBuilder() => _mock = new Mock<IPhotoService>();

        public PhotoServiceBuilder UploadImage(IFormFile? file, User user, string filename)
        {
            if (file is not null)
            {
                _mock.Setup(x => x.UploadImage(file, It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(new ImageUploadResultDto
                {
                    PublicId = $"{user.UserIdentifier}/{filename}",
                    Url = new Faker().Internet.Url()
                });
            }

            return this;
        }

        public IPhotoService Build() => _mock.Object;
    }
}
