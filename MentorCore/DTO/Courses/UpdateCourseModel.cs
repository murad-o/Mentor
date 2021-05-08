namespace MentorCore.DTO.Courses
{
    public record UpdateCourseModel
    {
        public string Name { get; init; }
        public string Description { get; init; }
    }
}
