using Common.Enums;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.IdentityModels
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryDate { get; set; }
        public UserType UserType { get; set; }
    }
}
