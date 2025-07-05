using Application.Services.AssignmentSubmissions;
using Common.Authorization;
using Common.Request.AssignmentSubmissions;
using Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementSystem.Controllers.AssignmentSubmissions
{
    public class AssignmentSubmissionController(IAssignmentSubmissionService assignmentSubmissionService) : BaseController
    {
        private readonly IAssignmentSubmissionService _assignmentSubmissionService = assignmentSubmissionService;

        [HttpGet("get-all-assignment-submission")]
        [Authorize(Roles = AppRole.Teacher)]
        public async Task<IActionResult> GetAllAssignmentSubmissionAsync(int pageIndex, int pageSize)
              => Ok(await _assignmentSubmissionService.GetAllAssignmentSubmissionAsync(pageIndex, pageSize));


        [HttpPost("submit-assignment")]
        [Authorize(Roles = AppRole.Student)]
        public async Task<IActionResult> AddSubmitAssignmentAsync([FromBody] AddAssignmentSubmissionRequest request, CancellationToken token)
         => Ok(await _assignmentSubmissionService.SubmitAssignmentAsync(request, token));


    }
}
