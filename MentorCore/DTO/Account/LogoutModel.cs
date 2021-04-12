namespace MentorCore.DTO.Account
{
    public record LogoutModel
    {
        public string RefreshToken { get; init; }
    }
}
