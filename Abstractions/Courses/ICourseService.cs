using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.Courses;

namespace Abstractions.Courses
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
