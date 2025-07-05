namespace Application.DTOs.Enrollments
{
    public class EnrollmentDTO
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public List<EnrollmentItemDTO> Enrollments { get; set; }
    }

    public class EnrollmentItemDTO
    {
        public Guid EnrollmentId { get; set; }
        public Guid CourseId { get; set; }
        public string? CourseName { get; set; }
        public string? CourseDescription { get; set; }

        public DateTime EnrolledDate { get; set; }
    }
}
