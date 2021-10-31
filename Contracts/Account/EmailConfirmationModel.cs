namespace Contracts.Account
{
    public record EmailConfirmationModel
    {
        public string Email { get; init; }
        public string Token { get; init; }
    }
}
