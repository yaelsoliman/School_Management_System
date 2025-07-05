namespace Domain.Entities
{
    public class Grade : BaseEntity
    {
        public float Score { get; set; }
        public string? Feedback { get; set; }

        public Guid? AssignmentSubmissionId { get; set; } 
        public AssignmentSubmission? AssignmentSubmission { get; set; }

        public Guid TeacherId { get; set; }
        public Guid StudentId { get; set; }


    }
}
