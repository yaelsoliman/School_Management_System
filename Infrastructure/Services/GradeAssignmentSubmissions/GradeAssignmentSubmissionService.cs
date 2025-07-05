using Application;
using Application.DTOs.GradeAssignmentSubmissions;
using Application.Services.GradeAssignmentSubmissions;
using Application.Services.Identity;
using AutoMapper;
using Common.Exceptions;
using Common.Request.GradeAssignmentSubmissions;
using Common.Responses.Wrappers;
using Domain.Entities;
using Infrastructure.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.GradeAssignmentSubmissions
{
    public class GradeAssignmentSubmissionService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser, UserManager<User> userManager, ILogger<GradeAssignmentSubmissionService> logger) : IGradeAssignmentSubmissionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUser = currentUser;
        private readonly UserManager<User> _userManager = userManager;
        private readonly ILogger<GradeAssignmentSubmissionService> _logger = logger;

        public async Task<ResponseWrapper<GradeAssignmentSubmissionsDTO>> GradeAssignmentAsync(AddGradeAssignmentSubmissionRequest request, CancellationToken token)
        {
            try
            {
                _logger.LogInformation("GradeAssignmentAsync started at {Time}", DateTime.Now);
                var currentUserId = _currentUser.UserId;

                if (string.IsNullOrWhiteSpace(currentUserId))
                    return await ResponseWrapper<GradeAssignmentSubmissionsDTO>.FailAsync("User not found");

                var user = await _userManager.FindByIdAsync(currentUserId)
                    ?? throw new UserFriendlyException("User not found", null);

                if (user.UserType != Common.Enums.UserType.Teacher)
                    return await ResponseWrapper<GradeAssignmentSubmissionsDTO>.FailAsync("Only teachers can grade assignments.");

                var submission = await _unitOfWork.AssignmentSubmissionRepository.GetByIdAsync(request.AssignmentSubmissionId);

                if (submission is null)
                    return await ResponseWrapper<GradeAssignmentSubmissionsDTO>.FailAsync("Assignment submission not found.");

                if (!submission.IsSubmitted)
                    return await ResponseWrapper<GradeAssignmentSubmissionsDTO>.FailAsync("This assignment has not been submitted yet.");

                var student = await _userManager.FindByIdAsync(submission.StudentId.ToString());
                if (student is null)
                    return await ResponseWrapper<GradeAssignmentSubmissionsDTO>.FailAsync("student not found");

                var grade = new Grade
                {
                    AssignmentSubmissionId = submission.Id,
                    TeacherId = Guid.Parse(currentUserId),
                    Score = request.Score,
                    Feedback = request.Feedback,
                    StudentId = Guid.Parse(student.Id)
                };

                await _unitOfWork.ExecuteTransactionAsync(async () => await _unitOfWork.GradeRepository.AddAsync(grade), token);
                _logger.LogInformation("GradeAssignmentAsync executed successfully for student {StudentId}", submission.StudentId);

                var gradeDto = new GradeAssignmentSubmissionsDTO
                {
                    Id = grade.Id,
                    AssignmentSubmissionId = grade.AssignmentSubmissionId.Value,
                    StudentId = Guid.Parse(student.Id),
                    StudentName = !string.IsNullOrWhiteSpace(student.UserName)
                        ? student.UserName
                        : $"{student.FirstName?.Trim()} {student.LastName?.Trim()}".Trim(),
                    StudentEmail = student.Email,
                    Score = grade.Score,
                    Feedback = grade.Feedback,
                    TeacherId = grade.TeacherId
                };

                return await ResponseWrapper<GradeAssignmentSubmissionsDTO>.SuccessAsync(gradeDto, "Assignment graded successfully.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Adding grade assignment submission the course ");
                return await ResponseWrapper<GradeAssignmentSubmissionsDTO>.FailAsync("An unexpected error occurred. Please try again later.");

            }
        }
    }
}
