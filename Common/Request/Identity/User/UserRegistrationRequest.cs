using Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Common.Request.Identity.User
{
    public class UserRegistrationRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
        public bool AutoConfirmEmail { get; set; }

        public UserTypeRequest UserTypeRequest { get; set; }
    }
}
