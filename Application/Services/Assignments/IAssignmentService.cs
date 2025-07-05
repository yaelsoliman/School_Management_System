using Application.DTOs.Assignments;
using Common.Request.Assignments;
using Common.Responses.Wrappers;

namespace Application.Services.Assignments
{
    public interface IAssignmentService
    {
        Task<ResponseWrapper<AssignmentDTO>> LinkAssignmentToCourseAsync(AddAssignmentRequest request, CancellationToken token);
        Task<ResponseWrapper<AssignmentDTO>> GetAssignmentByCourseIdAsync(Guid courseId);

    }
}
