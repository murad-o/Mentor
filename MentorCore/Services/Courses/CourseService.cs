using System.Threading.Tasks;
using Entities.Data;
using Entities.Models;
using MentorCore.DTO.Courses;
using MentorCore.Interfaces.Account;
using MentorCore.Interfaces.Courses;
using Microsoft.EntityFrameworkCore;

namespace MentorCore.Services.Courses
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public async Task<Course> GetCourseAsync(int id)
        {
            return await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == id);
        }

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

        public async Task UpdateCourseAsync(Course course, UpdateCourseModel updateCourseModel)
        {
            course.Name = updateCourseModel.Name;
            course.Description = updateCourseModel.Description;

            _dbContext.Attach(course).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public bool IsUserOwner(User user, Course course)
        {
            return user.Id == course.UserId;
        }
    }
}
