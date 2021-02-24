using System.ComponentModel.DataAnnotations;

namespace MentorCore.DTO.Account
{
    public record RegisterModel
    {
        [Required]
        public string Name { get; init; }

        [Required]
        [EmailAddress]
        public string Email { get; init; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; init; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; init; }
    }
}
