using Common.Request.Identity.Token;
using Common.Responses;
using Common.Responses.Wrappers;

namespace Application.Services.Identity.Token
{
    public interface ITokenService
    {
        Task<ResponseWrapper<TokenResponse>> GetTokenAsync(TokenRequest tokenRequest);
        Task<ResponseWrapper<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);

    }
}
