using Application;
using Application.Repository;
using Application.Services.Assignments;
using Application.Services.AssignmentSubmissions;
using Application.Services.Courses;
using Application.Services.Enrollments;
using Application.Services.GradeAssignmentSubmissions;
using Application.Services.Identity;
using Application.Services.Identity.Token;
using Application.Services.Identity.User;
using Infrastructure.Context;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services.Assignments;
using Infrastructure.Services.AssignmentSubmissions;
using Infrastructure.Services.Courses;
using Infrastructure.Services.GradeAssignmentSubmissions;
using Infrastructure.Services.Identity;
using Infrastructure.Services.Identity.Token;
using Infrastructure.Services.Identity.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<ApplicationDbSeeder>();

            return services;

        }
        public static void AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
        public static IServiceCollection AddInfrastructuresService(this IServiceCollection services)
        {
            services
                .AddTransient<ITokenService, TokenService>()
                .AddTransient<IUserService, UserService>()
                .AddTransient<ICourseService, CourseService>()
                .AddTransient<IEnrollmentService, EnrollmentService>()
                .AddTransient<IAssignmentService, AssignmentService>()
                .AddTransient<IAssignmentSubmissionService, AssignmentSubmissionService>()
                .AddTransient<IGradeAssignmentSubmissionService, GradeAssignmentSubmissionService>()

                .AddScoped<ICourseRepository, CourseRepository>()
                .AddScoped<IAssignmentRepository, AssignmentRepository>()
                .AddScoped<IEnrollmentRepository, EnrollmentRepository>()
                .AddScoped<IGradeRepository, GradeRepository>()
                .AddScoped<IAssignmentSubmissionRepository, AssignmentSubmissionRepository>()
                .AddScoped<ICurrentUserService, CurrentUserService>()

                .AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
