using Application.Repository;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repository;

namespace Infrastructure.Repositories;

public class GradeRepository(ApplicationDbContext context) : GenericRepository<Grade>(context), IGradeRepository
{
}
