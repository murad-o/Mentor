using System.Threading.Tasks;
using MentorCore.DTO.Account;

namespace MentorCore.Interfaces.Account
{
    public interface IAccountService
    {
        Task SignUpAsync(RegisterModel registerModel);
        Task<JwtTokenModel> SignInAsync(LoginModel loginModel);
        Task SignOutAsync(LogoutModel logoutModel);
        Task ConfirmEmailAsync(EmailConfirmationModel emailModel);
    }
}
