using System.Threading.Tasks;
using Entities.Models;
using MentorCore.DTO.Courses;

namespace MentorCore.Interfaces.Courses
{
    public interface ICourseService
    {
        Task<Course> GetCourseAsync(int id);
        Task CreateCourseAsync(Course course);
        Task UpdateCourseAsync(Course course, UpdateCourseModel updateCourseModel);
        bool IsUserOwner(User user, Course course);
    }
}
