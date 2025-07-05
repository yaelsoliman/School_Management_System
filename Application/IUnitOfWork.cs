using Application.Repository;

namespace Application;

public interface IUnitOfWork
{
    ICourseRepository CourseRepository { get; }
    IEnrollmentRepository EnrollmentRepository { get; }
    IAssignmentRepository AssignmentRepository { get; }
    IGradeRepository GradeRepository { get; }
    IAssignmentSubmissionRepository AssignmentSubmissionRepository { get; }

    Task SaveChangesAsync(CancellationToken token);
    Task ExecuteTransactionAsync(Action action, CancellationToken token);
    Task ExecuteTransactionAsync(Func<Task> action, CancellationToken token);
}
