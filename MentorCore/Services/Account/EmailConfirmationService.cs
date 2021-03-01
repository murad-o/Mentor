using System;
using System.Threading.Tasks;
using Entities.Models;
using MentorCore.DTO.Account;
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

        public async Task<IdentityResult> ConfirmEmailAsync(EmailConfirmationModel emailModel)
        {
            var user = await _userManager.FindByEmailAsync(emailModel.Email);

            if (user is null)
                throw new ArgumentException("User is not found");

            return await _userManager.ConfirmEmailAsync(user, emailModel.Token);
        }
    }
}
