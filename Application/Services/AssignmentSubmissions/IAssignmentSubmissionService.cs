using Application.DTOs.AssignmentSubmissions;
using Common;
using Common.Request.AssignmentSubmissions;
using Common.Responses.Wrappers;

namespace Application.Services.AssignmentSubmissions
{
    public interface IAssignmentSubmissionService
    {
        Task<ResponseWrapper<AssignmentSubmissionDTO>> SubmitAssignmentAsync(AddAssignmentSubmissionRequest request, CancellationToken token);
        Task<ResponseWrapper<Pagination<AssignmentSubmissionDTO>>> GetAllAssignmentSubmissionAsync(int pageIndex, int pageSize);
    }
}
