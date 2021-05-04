using System.Threading.Tasks;
using Entities.Data;
using Entities.Models;
using MentorCore.Interfaces.Courses;

namespace MentorCore.Services.Courses
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _dbContext;

        public CourseService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateCourseAsync(Course course)
        {
            await _dbContext.Courses.AddAsync(course);
            await _dbContext.SaveChangesAsync();
        }
    }
}
