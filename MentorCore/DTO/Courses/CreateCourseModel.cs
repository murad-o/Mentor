using System.ComponentModel.DataAnnotations;

namespace MentorCore.DTO.Courses
{
    public record CreateCourseModel
    {
        [Required]
        public string Name { get; init; }

        [Required]
        public string Description { get; init; }
    }
}
