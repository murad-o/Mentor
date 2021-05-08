using System.ComponentModel.DataAnnotations;

namespace MentorCore.DTO.Courses
{
    public record CourseModel
    {
        [Required]
        public int Id { get; init; }

        [Required]
        public string Name { get; init; }

        [Required]
        public string Description { get; init; }
    }
}
