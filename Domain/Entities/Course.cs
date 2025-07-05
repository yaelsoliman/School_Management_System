namespace Domain.Entities
{
    public class Course : BaseEntity
    {
        public string? CourseName { get; set; }
        public string? CourseDescription { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        // Navigation Properties

        public virtual ICollection<Enrollment>? Enrollments { get; set; }
        public virtual ICollection<Assignment>? Assignments { get; set; }
    }
}
