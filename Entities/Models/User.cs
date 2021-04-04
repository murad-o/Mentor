using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Entities.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
