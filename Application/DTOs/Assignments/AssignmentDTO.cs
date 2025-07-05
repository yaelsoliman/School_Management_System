namespace Application.DTOs.Assignments
{
    public class AssignmentDTO : BaseDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public Guid? CourseId { get; set; }
        public Guid? CreatedByUserId { get; set; }
    }
}
