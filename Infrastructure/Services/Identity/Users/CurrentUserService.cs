using Application.Services.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Services.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext?.User;

            if (user != null)
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
                UserId = userIdClaim?.Value;
            }
            else
            {
                UserId = null;
            }
        }

        public string UserId { get; }

    }
}
