namespace Domain.Entities
{
    public class Enrollment : BaseEntity
    {
        public Guid? UserId { get; set; }
        public Guid? CourseId { get; set; }
        public virtual Course? Course { get; set; }
        public DateTime EnrolledDate { get; set; }
    }
}
