using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Entities.Data;
using Entities.Models;
using MentorCore.DTO.Courses;
using MentorCore.DTO.Users;
using MentorCore.Exceptions;
using MentorCore.Interfaces.Courses;
using MentorCore.Interfaces.Users;
using Microsoft.EntityFrameworkCore;

namespace MentorCore.Services.Courses
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public CourseService(AppDbContext dbContext, IMapper mapper, IUserService userService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<CourseModel> GetCourseAsync(int id)
        {
            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.Id == id);

            if (course is null)
                throw new NotFoundException("course is not found");

            var courseModel = _mapper.Map<CourseModel>(course);
            return courseModel;
        }

        public async Task<IEnumerable<CourseModel>> GetCoursesAsync()
        {
            var courses = await _dbContext.Courses.AsNoTracking().ToListAsync();

            var coursesModels = courses.Select(c => _mapper.Map<CourseModel>(c));
            return coursesModels;
        }

        public async Task CreateCourseAsync(CreateCourseModel courseModel)
        {
            var user = await _userService.GetCurrentUser();

            var course = new Course
            {
                Name = courseModel.Name,
                Description = courseModel.Description,
                UserId = user.Id
            };

            await _dbContext.Courses.AddAsync(course);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateCourseAsync(int courseId, UpdateCourseModel updateCourseModel)
        {
            var courseToUpdate = await _dbContext.Courses.FirstOrDefaultAsync(x => x.Id == courseId);

            if (courseToUpdate is null)
                throw new NotFoundException($"Course with id {courseId} not found");

            var currentUser = await _userService.GetCurrentUser();

            if (!IsUserOwner(currentUser, courseToUpdate))
                throw new BadRequestException();

            courseToUpdate.Name = updateCourseModel.Name;
            courseToUpdate.Description = updateCourseModel.Description;
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveCourseAsync(int courseId)
        {
            var course = await _dbContext.Courses.FirstOrDefaultAsync(x => x.Id == courseId);

            if (course is null)
                throw new NotFoundException($"Course with id {courseId} not found");

            var currentUser = await _userService.GetCurrentUser();

            if (!IsUserOwner(currentUser, course))
                throw new BadRequestException();

            _dbContext.Courses.Remove(course);
            await _dbContext.SaveChangesAsync();
        }

        private bool IsUserOwner(UserModel user, Course course)
        {
            return user.Id == course.UserId;
        }
    }
}
