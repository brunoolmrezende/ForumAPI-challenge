using Microsoft.AspNetCore.Http;

namespace Forum.Application.UseCases.User.Image
{
    public interface IAddUpdateImageUseCase
    {
        Task Execute(IFormFile file);
    }
}
