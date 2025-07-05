namespace Application.DTOs.Courses
{
    public class CourseDTO : BaseDTO
    {
        public string? CourseName { get; set; }
        public string? CourseDescription { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Guid? CreatedByUserId { get; set; }

    }
}
