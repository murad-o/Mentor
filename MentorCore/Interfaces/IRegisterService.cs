using System.Threading.Tasks;
using MentorCore.DTO.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MentorCore.Interfaces
{
    public interface IRegisterService
    {
        Task<IdentityResult> Register(RegisterModel registerModel);
    }
}