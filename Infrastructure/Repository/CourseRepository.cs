using Application.Repository;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repository;

namespace Infrastructure.Repositories;

public class CourseRepository(ApplicationDbContext context) : GenericRepository<Course>(context), ICourseRepository
{
}
