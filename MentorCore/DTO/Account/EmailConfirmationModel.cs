using System.ComponentModel.DataAnnotations;

namespace MentorCore.DTO.Account
{
    public record EmailConfirmationModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; }

        [Required]
        public string Token { get; init; }
    }
}
