using Application.DTOs.Courses;
using Common;
using Common.Request.Courses;
using Common.Responses.Wrappers;

namespace Application.Services.Courses
{
    public interface ICourseService
    {
        Task<ResponseWrapper<CourseDTO>> AddCourseAsync(AddCourseRequest request, CancellationToken token);
        Task<ResponseWrapper<CourseDTO>> UpdateCourseAsync(UpdateCourseRequest request, CancellationToken token);
        Task<ResponseWrapper<CourseDTO>> DeleteCourseAsync(Guid courseId, CancellationToken token);
        Task<ResponseWrapper<Pagination<CourseDTO>>> GetAllAsync(int pageIndex, int pageSize, string? searchTerm = null);

    }
}
