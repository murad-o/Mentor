using System.Threading.Tasks;
using MentorCore.DTO.Account;
using Microsoft.AspNetCore.Identity;

namespace MentorCore.Interfaces.Account
{
    public interface IEmailConfirmationService
    {
        Task<IdentityResult> ConfirmEmailAsync(EmailConfirmationModel emailModel);
    }
}
