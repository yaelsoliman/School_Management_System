using Application.Services.Enrollments;
using Common.Authorization;
using Common.Request.Enrollments;
using Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace SchoolManagementSystem.Controllers.Enrollments
{
    public class EnrollmentController(IEnrollmentService enrollmentService) : BaseController
    {
        private readonly IEnrollmentService enrollmentService = enrollmentService;

        [Authorize(Roles = AppRole.Admin)]
        [HttpPost("assign-student-to-courses")]
        public async Task<IActionResult> Add(AddEnrollmentRequest request, CancellationToken token)
            => Ok(await enrollmentService.AssignStudentToCoursesAsync(request, token));


    }
}
