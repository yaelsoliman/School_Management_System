using Application;
using Application.Repository;
using Infrastructure.Data;
using Infrastructure.Repositories;

namespace Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public ICourseRepository CourseRepository { get; }
    public IEnrollmentRepository EnrollmentRepository { get; }
    public IAssignmentRepository AssignmentRepository { get; }
    public IGradeRepository GradeRepository { get; }
    public IAssignmentSubmissionRepository AssignmentSubmissionRepository { get; }

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _context = dbContext;
        CourseRepository = new CourseRepository(_context);
        EnrollmentRepository = new EnrollmentRepository(_context);
        AssignmentRepository = new AssignmentRepository(_context);
        GradeRepository = new GradeRepository(_context);
        AssignmentSubmissionRepository = new AssignmentSubmissionRepository(_context);
    }
    public async Task SaveChangesAsync(CancellationToken token)
        => await _context.SaveChangesAsync(token);

    public async Task ExecuteTransactionAsync(Action action, CancellationToken token)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(token);
        try
        {
            action();
            await _context.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
            //We can record log here and retrieve const message
        }
    }

    public async Task ExecuteTransactionAsync(Func<Task> action, CancellationToken token)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(token);
        try
        {
            await action();
            await _context.SaveChangesAsync(token);
            await transaction.CommitAsync(token);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(token);
            //We can record log here and retrieve const message
        }
    }
}
