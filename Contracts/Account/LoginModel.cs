namespace Contracts.Account
{
    public record LoginModel
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }
}
