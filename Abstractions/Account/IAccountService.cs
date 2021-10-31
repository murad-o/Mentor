using System.Threading.Tasks;
using Contracts.Account;

namespace Abstractions.Account
{
    public interface IAccountService
    {
        Task SignUpAsync(RegisterModel registerModel);
        Task<JwtTokenModel> SignInAsync(LoginModel loginModel);
        Task SignOutAsync(LogoutModel logoutModel);
        Task ConfirmEmailAsync(EmailConfirmationModel emailModel);
    }
}
