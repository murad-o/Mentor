namespace MentorCore.DTO.Account
{
    public record RefreshTokenModel
    {
        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
    }
}
