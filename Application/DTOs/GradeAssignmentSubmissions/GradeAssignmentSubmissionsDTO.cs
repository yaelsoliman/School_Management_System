namespace Application.DTOs.GradeAssignmentSubmissions
{
    public class GradeAssignmentSubmissionsDTO : BaseDTO
    {
        public Guid AssignmentSubmissionId { get; set; }
        public float Score { get; set; }
        public string? Feedback { get; set; }
        public Guid TeacherId { get; set; }
        public Guid StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? StudentEmail { get; set; }
    }
}
