namespace MentorCore.DTO.Courses
{
    public record CourseModel
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
    }
}
