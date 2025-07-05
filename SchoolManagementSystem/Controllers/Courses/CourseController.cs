using Application.Services.Courses;
using Common.Authorization;
using Common.Request.Courses;
using Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementSystem.Controllers.Courses
{
    public class CourseController(ICourseService courseService) : BaseController
    {
        private readonly ICourseService _courseService = courseService;

        [Authorize(Roles = $"{AppRole.Admin},{AppRole.Teacher}")]
        [HttpPost("create-course")]
        public async Task<IActionResult> Add(AddCourseRequest request, CancellationToken token)
            => Ok(await _courseService.AddCourseAsync(request, token));

        [Authorize(Roles = $"{AppRole.Admin},{AppRole.Teacher}")]
        [HttpPut("update-course")]
        public async Task<IActionResult> Update(UpdateCourseRequest request, CancellationToken token)
            => Ok(await _courseService.UpdateCourseAsync(request, token));


        [Authorize(Roles = $"{AppRole.Admin},{AppRole.Teacher}")]
        [HttpDelete("delete-course")]
        public async Task<IActionResult> Delete(Guid courseId, CancellationToken token)
            => Ok(await _courseService.DeleteCourseAsync(courseId, token));

        [Authorize(Roles = $"{AppRole.Admin},{AppRole.Teacher},{AppRole.Student}")]
        [HttpGet("get-all-courses")]
        public async Task<IActionResult> GetAllAsync(int pageIndex, int pageSize, string? searchTerm = null)
            => Ok(await _courseService.GetAllAsync(pageIndex, pageSize, searchTerm));


    }
}
