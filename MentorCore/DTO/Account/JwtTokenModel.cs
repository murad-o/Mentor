namespace MentorCore.DTO.Account
{
    public record JwtTokenModel
    {
        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
    }
}
