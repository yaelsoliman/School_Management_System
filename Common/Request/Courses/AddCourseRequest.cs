using System.ComponentModel.DataAnnotations;

namespace Common.Request.Courses
{
    public class AddCourseRequest
    {
        public string? CourseName { get; set; }

        public string? CourseDescription { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

    }
}
