using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class RefreshToken
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public DateTime ExpireTime { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
