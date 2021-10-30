using System.Threading.Tasks;
using MentorCore.DTO.Users;

namespace MentorCore.Interfaces.Users
{
    public interface IUserService
    {
        Task<UserModel> GetCurrentUser();
    }
}
