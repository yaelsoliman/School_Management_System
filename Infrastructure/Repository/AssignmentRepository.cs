using Application.Repository;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repository;

namespace Infrastructure.Repositories;

public class AssignmentRepository(ApplicationDbContext context) : GenericRepository<Assignment>(context), IAssignmentRepository
{
}
