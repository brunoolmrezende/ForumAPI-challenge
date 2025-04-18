using Forum.Communication.Request;
using Forum.Communication.Response;

namespace Forum.Application.UseCases.Token
{
    public interface IUseRefreshTokenUseCase
    {
        Task<ResponseTokensJson> Execute(RequestNewTokenJson request);
    }
}
