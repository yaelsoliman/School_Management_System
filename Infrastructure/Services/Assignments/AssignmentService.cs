using Application;
using Application.DTOs.Assignments;
using Application.Services.Assignments;
using Application.Services.Identity;
using AutoMapper;
using Common.Request.Assignments;
using Common.Responses.Wrappers;
using Domain.Entities;
using Infrastructure.Services.Courses;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Assignments
{
    public class AssignmentService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ILogger<AssignmentService> logger) : IAssignmentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly ILogger<AssignmentService> _logger = logger;

        public async Task<ResponseWrapper<AssignmentDTO>> GetAssignmentByCourseIdAsync(Guid courseId)
        {
            try
            {
                _logger.LogInformation("GetAssignmentByCourseIdAsync started at {Time}", DateTime.Now);

                if (courseId == Guid.Empty)
                    return await ResponseWrapper<AssignmentDTO>.FailAsync("please provide courseId value");

                var course = await _unitOfWork.CourseRepository.GetByIdAsync(courseId);
                if (course == null)
                    return await ResponseWrapper<AssignmentDTO>.FailAsync("course not found");

                var assignment = await _unitOfWork.AssignmentRepository.FirstOrDefaultAsync(x => x.CourseId == course.Id);
                if (assignment is null)
                    return await ResponseWrapper<AssignmentDTO>.FailAsync("assignment not found");

                var assignmentDto = _mapper.Map<AssignmentDTO>(assignment);
                _logger.LogInformation("GetAssignmentByCourseIdAsync executed Successfully");

                return await ResponseWrapper<AssignmentDTO>.SuccessAsync(assignmentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while get assignment by courseId the course ");
                return await ResponseWrapper<AssignmentDTO>.FailAsync("An unexpected error occurred. Please try again later.");
            }
        }

        public async Task<ResponseWrapper<AssignmentDTO>> LinkAssignmentToCourseAsync(AddAssignmentRequest request, CancellationToken token)
        {
            try
            {
                _logger.LogInformation("LinkAssignmentToCourseAsync started at {Time}", DateTime.Now);

                var course = await _unitOfWork.CourseRepository.GetByIdAsync(request.CourseId.Value);
                if (course == null)
                    return await ResponseWrapper<AssignmentDTO>.FailAsync("course not found");

                var assignment = _mapper.Map<Assignment>(request);
                assignment.CreatedByUserId = Guid.Parse(_currentUserService.UserId);
                await _unitOfWork.ExecuteTransactionAsync(async () => await _unitOfWork.AssignmentRepository.AddAsync(assignment), token);

                _logger.LogInformation("LinkAssignmentToCourseAsync executed Successfully");

                var assignmentCreatedDTO = _mapper.Map<AssignmentDTO>(assignment);
                return await ResponseWrapper<AssignmentDTO>.SuccessAsync(assignmentCreatedDTO, "assignment created succesfully ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while link assignment to course ");
                return await ResponseWrapper<AssignmentDTO>.FailAsync("An unexpected error occurred. Please try again later.");
            }
        }
    }
}
