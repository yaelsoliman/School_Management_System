using Microsoft.AspNetCore.Identity;

namespace Infrastructure.IdentityModels
{
    public class Role : IdentityRole
    {
        public string? Description { get; set; }
    }
}
