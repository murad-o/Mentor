namespace Contracts.Account
{
    public record LogoutModel
    {
        public string RefreshToken { get; init; }
    }
}
