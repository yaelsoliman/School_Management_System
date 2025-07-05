using Application.Services.Identity.User;
using Common.Authorization;
using Common.Request.Identity.User;
using Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementSystem.Controllers.Identity
{
    public class UserController(IUserService userService) : BaseController
    {
        private readonly IUserService _userService = userService;

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationRequest userRegistration)
        {
            var response = await _userService.RegisterUserAsync(userRegistration);
            if (response.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpPost("register-by-admin")]
        [Authorize(Roles = AppRole.Admin)]
        public async Task<IActionResult> RegisterAdminUser([FromBody] AdminUserRegistrationRequest adminRegistration)
        {
            var response = await _userService.RegisterByAdminAsync(adminRegistration);
            if (response.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("get-all-students")]
        [Authorize(Roles = $"{AppRole.Admin},{AppRole.Teacher}")]
        public async Task<IActionResult> GetAllStudents()
        {
            var response = await _userService.GetAllStudentAsync();
            if (response.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
    }
}
