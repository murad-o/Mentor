using System.Threading.Tasks;
using MentorCore.DTO.Account;
using Microsoft.AspNetCore.Identity;

namespace MentorCore.Interfaces
{
    public interface IRegisterService
    {
        Task<IdentityResult> Register(RegisterModel registerModel);
    }
}
