using System.Threading.Tasks;
using Entities.Models;

namespace MentorCore.Interfaces.Courses
{
    public interface ICourseService
    {
        Task CreateCourseAsync(Course course);
    }
}
