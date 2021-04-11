namespace MentorCore.Models.JWT
{
    public class JwtConfiguration
    {
        public string ValidIssuer { get; init; }
        public string ValidAudience { get; init; }
        public int LifeTime { get; init; }
        public string SecretKey { get; init; }
    }
}
