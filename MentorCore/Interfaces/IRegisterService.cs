using System.Threading.Tasks;
using MentorCore.DTO.Account;

namespace MentorCore.Interfaces
{
    public interface IRegisterService
    {
        Task RegisterAsync(RegisterModel registerModel);
    }
}
