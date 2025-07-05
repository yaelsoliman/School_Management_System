using System.ComponentModel.DataAnnotations;

namespace Common.Request.Enrollments
{
    public class AddEnrollmentRequest
    {
        public Guid UserId { get; set; }

        public List<Guid> CourseIds { get; set; }

        public DateTime EnrolledDate { get; set; }
    }
}
