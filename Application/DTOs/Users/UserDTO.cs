using Common.Enums;

namespace Application.DTOs.Users
{
    public class UserDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryDate { get; set; }
        public UserType UserType { get; set; }
    }
}
