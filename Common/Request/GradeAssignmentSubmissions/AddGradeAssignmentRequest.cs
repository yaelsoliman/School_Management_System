namespace Common.Request.GradeAssignmentSubmissions
{
    public class AddGradeAssignmentSubmissionRequest
    {
        public Guid AssignmentSubmissionId { get; set; }
        public float Score { get; set; }
        public string? Feedback { get; set; }
    }
}
