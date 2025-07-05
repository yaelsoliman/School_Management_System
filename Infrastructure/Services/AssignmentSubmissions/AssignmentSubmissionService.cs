using Application;
using Application.DTOs.Assignments;
using Application.DTOs.AssignmentSubmissions;
using Application.Services.AssignmentSubmissions;
using Application.Services.Identity;
using AutoMapper;
using Common;
using Common.Enums;
using Common.Exceptions;
using Common.Request.AssignmentSubmissions;
using Common.Responses.Wrappers;
using Domain.Entities;
using Infrastructure.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Services.AssignmentSubmissions
{
    public class AssignmentSubmissionService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser, UserManager<User> userManager, ILogger<AssignmentSubmissionService> logger) : IAssignmentSubmissionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUser = currentUser;
        private readonly UserManager<User> _userManager = userManager;
        private readonly ILogger<AssignmentSubmissionService> _logger = logger;

        public async Task<ResponseWrapper<Pagination<AssignmentSubmissionDTO>>> GetAllAssignmentSubmissionAsync(int pageIndex, int pageSize)
        {
            try
            {
                _logger.LogInformation("GetAllAssignmentSubmissionAsync started at {Time}", DateTime.Now);

                var submissions = await _unitOfWork.AssignmentSubmissionRepository.ToPagination(
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    orderBy: x => x.SubmissionDate,
                    ascending: true,
                    selector: x => new AssignmentSubmissionDTO
                    {
                        Id = x.Id,
                        StudentId = x.StudentId,
                        SubmissionDate = x.SubmissionDate,
                    }
                );

                if (submissions.Items == null || !submissions.Items.Any())
                {
                    return await ResponseWrapper<Pagination<AssignmentSubmissionDTO>>.SuccessAsync(submissions, "No submissions found.");
                }
                _logger.LogInformation("count of {submissons} ", submissions.Items.Count);

                var studentIds = submissions.Items.Select(x => x.StudentId.ToString()).ToList();
                _logger.LogInformation("{studentIds} ", studentIds);

                var students = await _userManager.Users
                    .Where(u => studentIds.Contains(u.Id) && u.UserType == UserType.Student)
                    .ToListAsync();

                _logger.LogInformation("{students} ", students);


                foreach (var submission in submissions.Items)
                {
                    var student = students.FirstOrDefault(s => s.Id == submission.StudentId.ToString());

                    if (student != null)
                    {
                        submission.StudentName = !string.IsNullOrWhiteSpace(student.UserName)
                            ? student.UserName
                            : $"{student.FirstName?.Trim()} {student.LastName?.Trim()}".Trim();
                        submission.StudentEmail = student.Email;
                    }
                    else
                    {
                        submission.StudentName = "Unknown Student";
                    }
                }
                _logger.LogInformation("GetAllAssignmentSubmissionAsync executed Successfully");

                return await ResponseWrapper<Pagination<AssignmentSubmissionDTO>>.SuccessAsync(submissions, "Submissions fetched successfully");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while get the assignments submission ");
                return await ResponseWrapper<Pagination<AssignmentSubmissionDTO>>.FailAsync("An unexpected error occurred. Please try again later.");

            }
        }

        public async Task<ResponseWrapper<AssignmentSubmissionDTO>> SubmitAssignmentAsync(AddAssignmentSubmissionRequest request, CancellationToken token)
        {
            try
            {
                _logger.LogInformation("SubmitAssignmentAsync started at {Time}", DateTime.Now);

                var currentUserId = _currentUser.UserId;

                if (string.IsNullOrWhiteSpace(currentUserId))
                    return await ResponseWrapper<AssignmentSubmissionDTO>.FailAsync("User not found");

                var user = await _userManager.FindByIdAsync(currentUserId)
                    ?? throw new UserFriendlyException("User not found", null);

                if (user.UserType != UserType.Student)
                    return await ResponseWrapper<AssignmentSubmissionDTO>.FailAsync("Only students can submit assignments.");

                var assignment = await _unitOfWork.AssignmentRepository.GetByIdAsync(request.AssignmentId);
                if (assignment == null)
                    return await ResponseWrapper<AssignmentSubmissionDTO>.FailAsync("Assignment not found.");

                var existingSubmission = await _unitOfWork.AssignmentSubmissionRepository
                    .FirstOrDefaultAsync(s => s.AssignmentId == request.AssignmentId && s.StudentId == Guid.Parse(currentUserId));

                if (existingSubmission != null)
                    return await ResponseWrapper<AssignmentSubmissionDTO>.FailAsync("You have already submitted this assignment.");

                var submission = new AssignmentSubmission
                {
                    AssignmentId = request.AssignmentId,
                    StudentId = Guid.Parse(currentUserId),
                    SubmissionDate = DateTime.UtcNow,
                    IsSubmitted = true
                };

                await _unitOfWork.ExecuteTransactionAsync(async () => await _unitOfWork.AssignmentSubmissionRepository.AddAsync(submission), token);

                _logger.LogInformation("SubmitAssignmentAsync executed Successfully");


                var submissionDto = new AssignmentSubmissionDTO
                {
                    Id = submission.Id,
                    StudentId = Guid.Parse(user.Id),
                    StudentName = !string.IsNullOrWhiteSpace(user.UserName)
                        ? user.UserName
                        : $"{user.FirstName?.Trim()} {user.LastName?.Trim()}".Trim(),
                    StudentEmail = user.Email,
                    SubmissionDate = submission.SubmissionDate,
                    Assignment = _mapper.Map<AssignmentDTO>(assignment)
                };

                return await ResponseWrapper<AssignmentSubmissionDTO>.SuccessAsync(submissionDto, "Assignment submitted successfully.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while submit assignment async");
                return await ResponseWrapper<AssignmentSubmissionDTO>.FailAsync("An unexpected error occurred. Please try again later.");

            }
        }
    }
}
