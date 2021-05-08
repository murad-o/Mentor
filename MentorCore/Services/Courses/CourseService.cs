using System.Threading.Tasks;
using Entities.Data;
using Entities.Models;
using MentorCore.Interfaces.Account;
using MentorCore.Interfaces.Courses;

namespace MentorCore.Services.Courses
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public CourseService(AppDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task CreateCourseAsync(Course course)
        {
            var user = await _currentUserService.GetCurrentUser();
            course.User = user;

            await _dbContext.Courses.AddAsync(course);
            await _dbContext.SaveChangesAsync();
        }
    }
}
