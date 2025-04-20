using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Forum.Domain.Dtos;
using Forum.Domain.Entities;
using Forum.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace Forum.Infrastructure.Services.Photo
{
    public class PhotoService(Cloudinary cloudinary) : IPhotoService
    {
        private readonly Cloudinary _cloudinary = cloudinary;

        public async Task DeleteImage(string filename)
        {
            var deletionParams = new DeletionParams(filename);

            await _cloudinary.DestroyAsync(deletionParams);
        }

        public async Task<ImageUploadResultDto> UploadImage(IFormFile file, User user, string filename)
        {
            var uploadResult = new ImageUploadResult();

            var fileStream = file.OpenReadStream();

            if (fileStream.Length > 0)
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(filename, fileStream),
                    Folder = user.UserIdentifier.ToString(),
                    PublicId = filename,
                    Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return new ImageUploadResultDto
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.ToString()
            };
        }
    }
}
