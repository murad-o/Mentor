using System.Threading.Tasks;
using Entities.Models;
using MentorCore.Interfaces.Account;
using Microsoft.AspNetCore.Identity;

namespace MentorCore.Services.Account
{
    public class EmailConfirmationService : IEmailConfirmationService
    {
        private readonly UserManager<User> _userManager;

        public EmailConfirmationService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return IdentityResult.Failed();

            var emailConfirmed = await _userManager.ConfirmEmailAsync(user, token);
            return IdentityResult.Success;
        }
    }
}
