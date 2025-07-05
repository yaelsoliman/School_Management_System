using Application.DTOs.Courses;
using Application.DTOs.Enrollments;
using AutoMapper;
using Common.Enums;
using Common.Exceptions;
using Common.Request.Enrollments;
using Common.Responses;
using Common.Responses.Wrappers;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.IdentityModels;
using Infrastructure.Services.Courses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Application.Services.Enrollments
{
    public class EnrollmentService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, ApplicationDbContext dbContext, ILogger<EnrollmentService> logger) : IEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<User> _userManager = userManager;
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly ILogger<EnrollmentService> _logger = logger;

        public async Task<ResponseWrapper<EnrollmentDTO>> AssignStudentToCoursesAsync(AddEnrollmentRequest request, CancellationToken token)
        {
            try
            {
                _logger.LogInformation("AssignStudentToCoursesAsync started at {Time}", DateTime.Now);

                var user = await _userManager.FindByIdAsync(request.UserId.ToString())
                    ?? throw new UserFriendlyException("User not found", null);

                if (user.UserType != UserType.Student)
                    return await ResponseWrapper<EnrollmentDTO>.FailAsync("Only students can be enrolled in courses");

                var courses = await _dbContext.Courses
                    .Where(c => request.CourseIds.Contains(c.Id))
                    .ToListAsync(token);

                if (courses.Count == 0)
                    return await ResponseWrapper<EnrollmentDTO>.FailAsync("No valid courses found.");


                var existingEnrollments = await _dbContext.Enrollments
                    .Where(e => e.UserId == request.UserId && e.CourseId.HasValue && request.CourseIds.Contains(e.CourseId.Value))
                    .Select(e => e.CourseId)
                    .ToListAsync(token);

                var newEnrollments = new List<Enrollment>();

                foreach (var course in courses)
                {
                    if (existingEnrollments.Contains(course.Id))
                        return await ResponseWrapper<EnrollmentDTO>.FailAsync("student already in this course");

                    var enrollment = new Enrollment
                    {
                        UserId = request.UserId,
                        CourseId = course.Id,
                    };

                    newEnrollments.Add(enrollment);
                }

                if (newEnrollments.Any())
                {
                    await _unitOfWork.ExecuteTransactionAsync(async () => await _unitOfWork.EnrollmentRepository.AddRangeAsync(newEnrollments), token);
                    _logger.LogInformation("AssignStudentToCoursesAsync executed successfully for user {UserId}", request.UserId);
                    _logger.LogInformation("New enrollments: {@NewEnrollments}", newEnrollments);
                }

                _logger.LogInformation("No new enrollments were added for user {UserId}", request.UserId);


                var enrollmentItems = newEnrollments.Select(e => new EnrollmentItemDTO
                {
                    EnrollmentId = e.Id,
                    CourseId = e.CourseId.Value,
                    CourseName = courses.FirstOrDefault(c => c.Id == e.CourseId)?.CourseName,
                    CourseDescription = courses.FirstOrDefault(c => c.Id == e.CourseId)?.CourseDescription,
                    EnrolledDate = e.EnrolledDate
                }).ToList();

                var enrollmentDto = new EnrollmentDTO
                {
                    StudentId = request.UserId,
                    StudentName = !string.IsNullOrWhiteSpace(user.UserName)
                        ? user.UserName
                        : $"{user.FirstName?.Trim()} {user.LastName?.Trim()}".Trim(),
                    StudentEmail = user.Email,
                    Enrollments = enrollmentItems
                };
                return await ResponseWrapper<EnrollmentDTO>.SuccessAsync(enrollmentDto, "Assigned Student To Courses Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Add Student to course .");
                return await ResponseWrapper<EnrollmentDTO>.FailAsync("An unexpected error occurred. Please try again later.");

            }
        }
    }
}
