using System.ComponentModel.DataAnnotations;

namespace MentorCore.DTO.Account
{
    public record LoginModel
    {
        [Required]
        public string Email { get; init; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; init; }
    }
}
