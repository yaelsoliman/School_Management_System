using Application.DTOs.Users;
using Application.Services.Identity;
using Application.Services.Identity.User;
using AutoMapper;
using Common.Authorization;
using Common.Enums;
using Common.Request.Identity.User;
using Common.Responses.Wrappers;
using Infrastructure.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Identity.Users
{
    public class UserService(UserManager<User> userManager,IMapper mapper) : IUserService
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseWrapper<List<UserDTO>>> GetAllStudentAsync()
        {
            var users = await _userManager.Users
                .Where(x => x.UserType == UserType.Student)
                .AsNoTracking()
                .ToListAsync();

            var usersDTO = _mapper.Map<List<UserDTO>>(users);

            return await ResponseWrapper<List<UserDTO>>.SuccessAsync(usersDTO);
        }

        public async Task<IResponseWrapper> RegisterByAdminAsync(AdminUserRegistrationRequest request)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not null)
                return await ResponseWrapper.FailAsync("Email already taken");

            if (await _userManager.FindByNameAsync(request.UserName) is not null)
                return await ResponseWrapper.FailAsync("Username already taken");

            var newUser = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                EmailConfirmed = request.AutoConfirmEmail,
                UserType = request.UserType,
            };

            var identityResult = await _userManager.CreateAsync(newUser, request.Password);

            if (!identityResult.Succeeded)
                return await ResponseWrapper.FailAsync(GetIdentityResultErrorDecriptions(identityResult));

            string assignedRole = newUser.UserType switch
            {
                UserType.Admin => AppRole.Admin,
                UserType.Teacher => AppRole.Teacher,
                UserType.Student => AppRole.Student,
                _ => AppRole.Student
            };

            await _userManager.AddToRoleAsync(newUser, assignedRole);

            return await ResponseWrapper<string>.SuccessAsync($"User registered successfully as {assignedRole.ToLower()}.");
        }


        public async Task<IResponseWrapper> RegisterUserAsync(UserRegistrationRequest request)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not null)
                return await ResponseWrapper.FailAsync("Email already taken");

            if (await _userManager.FindByNameAsync(request.UserName) is not null)
                return await ResponseWrapper.FailAsync("Username already taken");

            UserType userTypeToSave = request.UserTypeRequest switch
            {
                UserTypeRequest.Teacher => UserType.Teacher,
                UserTypeRequest.Student => UserType.Student,
                _ => throw new ArgumentOutOfRangeException(nameof(request.UserTypeRequest), "Invalid user type request")
            };
            var newUser = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                EmailConfirmed = request.AutoConfirmEmail,
                UserType = userTypeToSave
            };

            //Hash Password
            var password = new PasswordHasher<User>();
            newUser.PasswordHash = password.HashPassword(newUser, request.Password);

            var identityResult = await _userManager.CreateAsync(newUser, request.Password);

            if (!identityResult.Succeeded)
                return await ResponseWrapper.FailAsync(GetIdentityResultErrorDecriptions(identityResult));

            string assignedRole = userTypeToSave switch
            {
                UserType.Student => AppRole.Student,
                UserType.Teacher => AppRole.Teacher,
                _ => AppRole.Student // fallback
            };

            await _userManager.AddToRoleAsync(newUser, assignedRole);

            return await ResponseWrapper<string>.SuccessAsync($"User registered successfully as {assignedRole.ToLower()} please login.");
        }

        private List<string> GetIdentityResultErrorDecriptions(IdentityResult identityResult)
        {
            var errorDescriptions = new List<string>();
            foreach (var error in identityResult.Errors)
            {
                errorDescriptions.Add(error.Description);
            }
            return errorDescriptions;
        }
    }
}
