using Common.Authorization;
using Infrastructure.Data;
using Infrastructure.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class ApplicationDbSeeder
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public ApplicationDbSeeder(ApplicationDbContext dbContext, UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task SeedDatabaseAsync()
        {
            // check for pending and apply migration if any
            await CheckAndApplyPendingMigrationAsync();
            //Seed Roles
            await SeedRolesAsync();

            //Seed Admin
            await SeedAdminUserAsync();


        }

        private async Task CheckAndApplyPendingMigrationAsync()
        {
            if (_dbContext.Database.GetPendingMigrations().Any())
            {
                await _dbContext.Database.MigrateAsync();
            }
        }


        private async Task SeedAdminUserAsync()
        {
            string adminUserName = AppCredentails.Email[..AppCredentails.Email.IndexOf("@")].ToLowerInvariant();
            var adminUser = new User()
            {
                FirstName = "Admin",
                LastName = "Admin",
                Email = AppCredentails.Email,
                UserName = adminUserName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                NormalizedEmail = AppCredentails.Email.ToUpperInvariant(),
                NormalizedUserName = adminUserName.ToUpperInvariant(),
            };
            if (!await _userManager.Users.AnyAsync(e => e.Email == AppCredentails.Email))
            {
                var password = new PasswordHasher<User>();
                adminUser.PasswordHash = password.HashPassword(adminUser, AppCredentails.Password);
                await _userManager.CreateAsync(adminUser);
            }

            if (!await _userManager.IsInRoleAsync(adminUser, AppRole.Admin))
            {
                await _userManager.AddToRoleAsync(adminUser, AppRole.Admin);
            }
        }
        private async Task SeedRolesAsync()
        {
            foreach (var roleName in AppRole.DefaultRoles)
            {
                if (await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == roleName)
                    is not Role role)
                {
                    role = new Role()
                    {
                        Name = roleName,
                        Description = $"{roleName} Role."
                    };

                    await _roleManager.CreateAsync(role);
                }
            }
        }
    }
}
