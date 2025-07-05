using Application.DTOs.Enrollments;
using Common.Request.Enrollments;
using Common.Responses.Wrappers;

namespace Application.Services.Enrollments
{
    public interface IEnrollmentService
    {
        Task<ResponseWrapper<EnrollmentDTO>> AssignStudentToCoursesAsync(AddEnrollmentRequest request,CancellationToken token);
    }
}
