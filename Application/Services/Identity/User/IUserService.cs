using Application.DTOs.Users;
using Common;
using Common.Request.Identity.User;
using Common.Responses.Wrappers;

namespace Application.Services.Identity.User
{
    public interface IUserService
    {
        Task<IResponseWrapper> RegisterUserAsync(UserRegistrationRequest request);
        Task<IResponseWrapper> RegisterByAdminAsync(AdminUserRegistrationRequest request);
        Task<ResponseWrapper<List<UserDTO>>> GetAllStudentAsync();
    }
}
