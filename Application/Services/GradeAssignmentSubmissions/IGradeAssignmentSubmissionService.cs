using Application.DTOs.GradeAssignmentSubmissions;
using Common.Request.GradeAssignmentSubmissions;
using Common.Responses.Wrappers;

namespace Application.Services.GradeAssignmentSubmissions
{
    public interface IGradeAssignmentSubmissionService
    {
        Task<ResponseWrapper<GradeAssignmentSubmissionsDTO>> GradeAssignmentAsync(AddGradeAssignmentSubmissionRequest request, CancellationToken token);
    }
}

