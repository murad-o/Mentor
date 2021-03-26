namespace MentorCore.DTO.Account
{
    public record JwtTokenService
    {
        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
    }
}
