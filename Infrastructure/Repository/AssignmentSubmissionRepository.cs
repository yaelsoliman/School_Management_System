using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repository;

namespace Application.Repository
{
    public class AssignmentSubmissionRepository(ApplicationDbContext context) : GenericRepository<AssignmentSubmission>(context), IAssignmentSubmissionRepository
    {
    }
}
