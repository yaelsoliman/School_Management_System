using Application.AppConfigs;
using Application.Services.Identity.Token;
using Common.Request.Identity.Token;
using Common.Responses.Wrappers;
using Common.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Infrastructure.IdentityModels;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Identity.Token
{
    public class TokenService(UserManager<User> userManager,
            RoleManager<Role> roleManager,
             IOptions<AppConfiguration> appConfiguration,
             ILogger<TokenService> logger) : ITokenService
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly RoleManager<Role> _roleManager = roleManager;
        private readonly AppConfiguration _appConfiguration = appConfiguration.Value;
        private readonly ILogger<TokenService> _logger = logger;

        public async Task<ResponseWrapper<TokenResponse>> GetTokenAsync(TokenRequest tokenRequest)
        {
            try
            {
                _logger.LogInformation("GetTokenAsync started at {Time}", DateTime.Now);

                // Validate user
                var user = await _userManager.FindByEmailAsync(tokenRequest.Email);
                // Check user
                if (user is null)
                {
                    _logger.LogInformation("Invalid password attempt for email: {Email}", tokenRequest.Email);
                    return await ResponseWrapper<TokenResponse>.FailAsync("Invalid Credentials.");

                }

                // Chcek email if email confirmed
                if (!user.EmailConfirmed)
                {
                    return await ResponseWrapper<TokenResponse>.FailAsync("Email not confirmed.");
                }
                // Check password
                var isPaswordValid = await _userManager.CheckPasswordAsync(user, tokenRequest.Password);
                if (!isPaswordValid)
                {
                    return await ResponseWrapper<TokenResponse>.FailAsync("Invalid Credentials.");
                }
                // generate refresh token
                user.RefreshToken = GenerateRefreshToken();
                user.RefreshTokenExpiryDate = DateTime.Now.AddDays(7);
                // Updated user
                await _userManager.UpdateAsync(user);

                // generate new token
                var token = await GenerateJWTAsync(user);

                var response = new TokenResponse
                {
                    Token = token,
                    RefreshToken = user.RefreshToken,
                    RefreshTokenExpiryTime = user.RefreshTokenExpiryDate
                };

                _logger.LogInformation("Token generated successfully.");

                return await ResponseWrapper<TokenResponse>.SuccessAsync(response);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generate the token ");
                return await ResponseWrapper<TokenResponse>.FailAsync("An unexpected error occurred. Please try again later.");

            }
        }
        public async Task<ResponseWrapper<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
        {
            try
            {
                _logger.LogInformation("GetRefreshTokenAsync started at {Time}", DateTime.Now);

                if (refreshTokenRequest is null)
                {
                    return await ResponseWrapper<TokenResponse>.FailAsync("Invalid Client Token.");
                }
                var userPrincipal = GetPrincipalFromExpiredToken(refreshTokenRequest.Token);
                var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
                var user = await _userManager.FindByEmailAsync(userEmail);

                if (user is null)
                    return await ResponseWrapper<TokenResponse>.FailAsync("User Not Found.");
                if (user.RefreshToken != refreshTokenRequest.RefreshToken || user.RefreshTokenExpiryDate <= DateTime.Now)
                    return await ResponseWrapper<TokenResponse>.FailAsync("Invalid Client Token.");

                var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
                user.RefreshToken = GenerateRefreshToken();
                await _userManager.UpdateAsync(user);

                var response = new TokenResponse
                {
                    Token = token,
                    RefreshToken = user.RefreshToken,
                    RefreshTokenExpiryTime = user.RefreshTokenExpiryDate
                };

                _logger.LogInformation("refresh token generated successfully.");

                return await ResponseWrapper<TokenResponse>.SuccessAsync(response);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while refresh the token ");
                return await ResponseWrapper<TokenResponse>.FailAsync("An unexpected error occurred. Please try again later.");
            }
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfiguration.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken
                || !jwtSecurityToken.Header.Alg
                .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateJWTAsync(User user)
        {
            var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
            return token;
        }

        private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
               claims: claims,
               expires: DateTime.UtcNow.AddMinutes(_appConfiguration.TokenExpiryInMinutes),
               signingCredentials: signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();

            var encryptedToken = tokenHandler.WriteToken(token);

            return encryptedToken;
        }
        private SigningCredentials GetSigningCredentials()
        {
            var secret = Encoding.UTF8.GetBytes(_appConfiguration.Secret);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }
        private async Task<IEnumerable<Claim>> GetClaimsAsync(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            var permissionClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
                var currentRole = await _roleManager.FindByNameAsync(role);
                var allPermissionsForCurrentRole = await _roleManager.GetClaimsAsync(currentRole);
                permissionClaims.AddRange(allPermissionsForCurrentRole);
            }
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier,user.Id),
                new(ClaimTypes.Email,user.Email),
                new(ClaimTypes.Name,user.FirstName),
                new(ClaimTypes.Surname,user.LastName),
                new(ClaimTypes.MobilePhone,user.PhoneNumber ?? string.Empty),
            }
            .Union(userClaims)
            .Union(roleClaims)
            .Union(permissionClaims);
            return claims;
        }
    }
}
