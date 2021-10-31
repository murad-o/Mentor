using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
