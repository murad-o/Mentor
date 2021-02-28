using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MentorCore.Interfaces.Account
{
    public interface IEmailConfirmationService
    {
        Task<IdentityResult> ConfirmEmailAsync(string email, string token);
    }
}
