using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Entities.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
