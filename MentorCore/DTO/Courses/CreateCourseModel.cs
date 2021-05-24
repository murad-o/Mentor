namespace MentorCore.DTO.Courses
{
    public record CreateCourseModel
    {
        public string Name { get; init; }
        public string Description { get; init; }
    }
}
