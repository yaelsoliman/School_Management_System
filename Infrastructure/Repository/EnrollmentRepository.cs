using Application.Repository;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repository;

namespace Infrastructure.Repositories;

public class EnrollmentRepository(ApplicationDbContext context) : GenericRepository<Enrollment>(context), IEnrollmentRepository
{
}
