using Application.DTOs.Assignments;

namespace Application.DTOs.AssignmentSubmissions
{
    public class AssignmentSubmissionDTO : BaseDTO
    {           
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public DateTime SubmissionDate { get; set; }
        public AssignmentDTO Assignment { get; set; }
    }
}
