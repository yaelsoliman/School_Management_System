using Application.Services.Assignments;
using Common.Authorization;
using Common.Request.Assignments;
using Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementSystem.Controllers.Assignments
{
    public class AssignmentController(IAssignmentService assignmentService) : BaseController
    {
        private readonly IAssignmentService _assignmentService = assignmentService;

        [HttpGet("get-assignment-by-courseId")]
        [Authorize(Roles = $"{AppRole.Teacher},{AppRole.Student}")]
        public async Task<IActionResult> GetAssignmentByCourseIdAsync(Guid courseId)
          => Ok(await _assignmentService.GetAssignmentByCourseIdAsync(courseId));


        [HttpPost("link-assignment-to-course")]
        [Authorize(Roles = AppRole.Teacher)]
        public async Task<IActionResult> LinkAssignmentToCourseAsync([FromBody] AddAssignmentRequest request, CancellationToken token)
         => Ok(await _assignmentService.LinkAssignmentToCourseAsync(request, token));

    }
}
