namespace Domain.Entities
{
    public class AssignmentSubmission : BaseEntity
    {
        public Guid? AssignmentId { get; set; }
        public virtual Assignment? Assignment { get; set; }
        public Guid StudentId { get; set; }
        public DateTime SubmissionDate { get; set; }
        public bool IsSubmitted { get; set; } = true;
    }
}
