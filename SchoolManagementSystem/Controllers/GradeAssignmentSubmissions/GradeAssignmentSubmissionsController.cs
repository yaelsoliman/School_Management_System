using Application.Services.GradeAssignmentSubmissions;
using Common.Authorization;
using Common.Request.GradeAssignmentSubmissions;
using Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementSystem.Controllers.GradeAssignmentSubmissions
{

    public class GradeAssignmentSubmissionsController(IGradeAssignmentSubmissionService gradeAssignmentSubmissionService) : BaseController
    {
        private readonly IGradeAssignmentSubmissionService _gradeAssignmentSubmissionService = gradeAssignmentSubmissionService;

        [HttpPost("add-grade-assignment-submission")]
        [Authorize(Roles = AppRole.Teacher)]
        public async Task<IActionResult> AddGradeAssignmentAsync([FromBody] AddGradeAssignmentSubmissionRequest request, CancellationToken token)
             => Ok(await _gradeAssignmentSubmissionService.GradeAssignmentAsync(request, token));

    }
}
