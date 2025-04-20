using Forum.Domain.Dtos;
using Forum.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Forum.Domain.Services
{
    public interface IPhotoService
    {
        Task<ImageUploadResultDto> UploadImage(IFormFile file, User user, string filename);
        Task DeleteImage(string filename);
    }
}
