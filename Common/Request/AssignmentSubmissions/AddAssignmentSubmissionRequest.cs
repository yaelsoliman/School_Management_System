namespace Common.Request.AssignmentSubmissions
{
    public class AddAssignmentSubmissionRequest
    {
        public Guid AssignmentId { get; set; }
        public string? Notes { get; set; }
    }
}
