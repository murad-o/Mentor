using System.Collections.Generic;
using System.Threading.Tasks;
using MentorCore.DTO.Courses;

namespace MentorCore.Interfaces.Courses
{
    public interface ICourseService
    {
        Task<CourseModel> GetCourseAsync(int id);
        Task<IEnumerable<CourseModel>> GetCoursesAsync();
        Task CreateCourseAsync(CreateCourseModel createCourseModel);
        Task UpdateCourseAsync(int courseId, UpdateCourseModel updateCourseModel);
        Task RemoveCourseAsync(int courseId);
    }
}
