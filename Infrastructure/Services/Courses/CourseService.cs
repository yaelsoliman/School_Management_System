using Application;
using Application.DTOs.Courses;
using Application.Services.Courses;
using Application.Services.Identity;
using AutoMapper;
using Common;
using Common.Request.Courses;
using Common.Responses.Wrappers;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Linq.Expressions;

namespace Infrastructure.Services.Courses
{
    public class CourseService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser, UserManager<User> userManager, ApplicationDbContext dbContext, ILogger<CourseService> logger) : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUser = currentUser;
        private readonly UserManager<User> _userManager = userManager;
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly ILogger<CourseService> _logger = logger;

        public async Task<ResponseWrapper<CourseDTO>> AddCourseAsync(AddCourseRequest request, CancellationToken token)
        {
            try
            {
                _logger.LogInformation("AddCourseAsync started at {Time}", DateTime.Now);
                var course = _mapper.Map<Course>(request);
                course.CreatedByUserId = Guid.Parse(_currentUser.UserId);
                await _unitOfWork.ExecuteTransactionAsync(async () => await _unitOfWork.CourseRepository.AddAsync(course), token);

                _logger.LogInformation("Course with ID {CourseId} created successfully.", course.Id);

                var courseCreatedDto = _mapper.Map<CourseDTO>(course);
                return await ResponseWrapper<CourseDTO>.SuccessAsync(courseCreatedDto, "Course Created Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Adding the course ");
                return await ResponseWrapper<CourseDTO>.FailAsync("An unexpected error occurred. Please try again later.");
            }
        }
        public async Task<ResponseWrapper<CourseDTO>> UpdateCourseAsync(UpdateCourseRequest request, CancellationToken token)
        {
            try
            {
                _logger.LogInformation("UpdateCourseAsync started at {Time}", DateTime.Now);

                var existCourse = await _unitOfWork.CourseRepository.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (existCourse is null)
                    return await ResponseWrapper<CourseDTO>.FailAsync("course not found");
                if (existCourse.CreatedByUserId != Guid.Parse(_currentUser.UserId))
                    return await ResponseWrapper<CourseDTO>.FailAsync("you cannot manage this course");

                _mapper.Map(request, existCourse);

                await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.CourseRepository.Update(existCourse), token);

                _logger.LogInformation("Course with ID {CourseId} updated successfully.", existCourse.Id);

                var courseUpdatedDto = _mapper.Map<CourseDTO>(existCourse);
                return await ResponseWrapper<CourseDTO>.SuccessAsync(courseUpdatedDto, "Course Updated Successfully");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the course with ID {CourseId}.", request.Id);
                return await ResponseWrapper<CourseDTO>.FailAsync("An unexpected error occurred. Please try again later.");
            }

        }
        public async Task<ResponseWrapper<CourseDTO>> DeleteCourseAsync(Guid courseId, CancellationToken token)
        {
            try
            {
                _logger.LogInformation("DeleteCourseAsync started at {Time}", DateTime.Now);

                var existCourse = await _unitOfWork.CourseRepository.FirstOrDefaultAsync(x => x.Id == courseId);
                if (existCourse is null)
                   
                    return await ResponseWrapper<CourseDTO>.FailAsync("course not found");
                if (existCourse.CreatedByUserId != Guid.Parse(_currentUser.UserId))
                    return await ResponseWrapper<CourseDTO>.FailAsync("you cannot manage this course");

                await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.CourseRepository.Delete(existCourse), token);
                _logger.LogInformation("Course with ID {CourseId} deleted successfully.", existCourse.Id);


                var courseDeletedDto = _mapper.Map<CourseDTO>(existCourse);
                return await ResponseWrapper<CourseDTO>.SuccessAsync(courseDeletedDto, "Course Deleted Successfully");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Removing the course ");
                return await ResponseWrapper<CourseDTO>.FailAsync("An unexpected error occurred. Please try again later.");

            }
        }

        public async Task<ResponseWrapper<Pagination<CourseDTO>>> GetAllAsync(int pageIndex, int pageSize, string? searchTerm = null)
        {
            try
            {
                _logger.LogInformation("GetAllCourseAsync started at {Time}", DateTime.Now);

                var currentUserId = _currentUser.UserId;

                if (string.IsNullOrWhiteSpace(currentUserId))
                    return await ResponseWrapper<Pagination<CourseDTO>>.FailAsync("User not found");

                var user = await _userManager.FindByIdAsync(currentUserId);
                if (user == null)
                    return await ResponseWrapper<Pagination<CourseDTO>>.FailAsync("User not found");

                Expression<Func<Course, bool>>? filter = null;

                if (user.UserType == Common.Enums.UserType.Student)
                {
                    var studentCourseIds = await _dbContext.Enrollments
                         .Where(e => e.UserId == Guid.Parse(currentUserId) && e.CourseId.HasValue)
                         .Select(e => e.CourseId.Value)
                         .ToListAsync();

                    filter = c => studentCourseIds.Contains(c.Id);
                }

                Expression<Func<Course, bool>>? searchPredicate = null;
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    searchPredicate = c =>
                        c.CourseName.Contains(searchTerm) ||
                        c.CourseDescription.Contains(searchTerm);
                }


                var courses = await _unitOfWork.CourseRepository.ToPagination(
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    filter: filter,
                    orderBy: x => x.StartDate,
                    ascending: true,
                    selector: x => new CourseDTO
                    {
                        Id = x.Id,
                        CourseName = x.CourseName,
                        CourseDescription = x.CourseDescription,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                    },
                    searchTerm: searchTerm,
                    searchPredicate: searchPredicate
                );
                _logger.LogInformation("GetAllCourseAsync executed successfully");

                return await ResponseWrapper<Pagination<CourseDTO>>.SuccessAsync(courses);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Get the courses");
                return await ResponseWrapper<Pagination<CourseDTO>>.FailAsync("An unexpected error occurred. Please try again later.");
            }
        }
    }
}
