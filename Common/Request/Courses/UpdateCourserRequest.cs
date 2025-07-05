namespace Common.Request.Courses
{
    public class UpdateCourseRequest
    {
        public Guid Id { get; set; }

        public string? CourseName { get; set; }

        public string? CourseDescription { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

    }
}
