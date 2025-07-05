using Application.Services.Identity.Token;
using Common.Request.Identity.Token;
using Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementSystem.Controllers.Identity
{
    public class TokenController(ITokenService tokenService) : BaseController
    {
        private readonly ITokenService _tokenService = tokenService;

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequest tokenRequest)
        {

            var response = await _tokenService.GetTokenAsync(tokenRequest);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("refresh-Token")]
        public async Task<IActionResult> GetRefreshTokenAsync([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            var response = await _tokenService.GetRefreshTokenAsync(refreshTokenRequest);
            if (response.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
    }
    
}
